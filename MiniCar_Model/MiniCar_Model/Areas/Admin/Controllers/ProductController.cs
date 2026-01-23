using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;
using System.Drawing;
using MiniCar_Model.Filters;


namespace MiniCar_Model.Areas.Admin.Controllers
{
  [Area("Admin")]
  [AdminAuthorize]
  public class ProductController : Controller
  {
    //------Tri Trong Trang
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
    {
      _context = context;
      _env = env;
    }
    //------
    public async Task<IActionResult> Index(string search, int page = 1)
    {
      int pageSize = 10;

      var query = _context.Products
          .Include(p => p.Category)
          .Include(p => p.ProductVariants)
          .AsQueryable();

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(p => p.NameProduct.Contains(search));
      }

      int totalItems = await query.CountAsync();

      var products = await query
        .OrderBy(p => p.ProductId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductIndexVM
        {
          ProductId = p.ProductId,
          NameProduct = p.NameProduct,
          CategoryName = p.Category!.NameCategory,

          MinPrice = p.ProductVariants.Any()
                ? p.ProductVariants.Min(v => v.Price)
                : 0,

          MaxPrice = p.ProductVariants.Any()
                ? p.ProductVariants.Max(v => v.Price)
                : 0,

          TotalQuantity = p.ProductVariants.Sum(v => v.Quantity),
          VariantCount = p.ProductVariants.Count,
          StatusProduct = p.StatusProduct
        })
        .ToListAsync();

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
      ViewBag.Search = search;

      return View(products);
    }

    //public async Task<IActionResult> Variants(int id)
    //{
    //  var product = await _context.Products
    //      .Include(p => p.ProductVariants)
    //        .ThenInclude(v => v.Color)
    //      .Include(p => p.ProductVariants)
    //        .ThenInclude(v => v.Size)
    //      .FirstOrDefaultAsync(p => p.ProductId == id);

    //  if (product == null)
    //  {
    //    TempData["Error"] = "Sản phẩm không tồn tại";
    //    return RedirectToAction("Index");
    //  }

    //  var variants = product.ProductVariants.Select(v => new ProductVariantIndexVM
    //  {
    //    VariantId = v.VariantId,
    //    SizeName = v.Size!.Scale,
    //    ColorName = v.Color!.ColorName,
    //    Price = v.Price,
    //    Quantity = v.Quantity,
    //    StatusVariant = v.StatusVariant
    //  }).ToList();

    //  ViewBag.ProductId = product.ProductId;
    //  ViewBag.ProductName = product.NameProduct;

    //  return View(variants);
    //}

    public async Task<IActionResult> Variants(int id, string? search, int page = 1)
    {
      const int pageSize = 10;

      var product = await _context.Products
          .AsNoTracking()
          .FirstOrDefaultAsync(p => p.ProductId == id);

      if (product == null)
      {
        TempData["Error"] = "Sản phẩm không tồn tại";
        return RedirectToAction("Index");
      }

      var query = _context.ProductVariants
          .AsNoTracking()
          .Where(v => v.ProductId == id);

      if (!string.IsNullOrWhiteSpace(search))
      {
        search = search.Trim();

        query = query.Where(v =>
            (v.Size != null && v.Size.Scale.Contains(search)) ||
            (v.Color != null && v.Color.ColorName.Contains(search)) ||
            v.StatusVariant.Contains(search)
        );
      }

      int totalItems = await query.CountAsync();

      var variants = await query
          .OrderBy(v => v.VariantId)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(v => new ProductVariantIndexVM
          {
            VariantId = v.VariantId,
            SizeName = v.Size!.Scale,
            ColorName = v.Color!.ColorName,
            Price = v.Price,
            Quantity = v.Quantity,
            StatusVariant = v.StatusVariant
          })
          .ToListAsync();

      ViewBag.ProductId = product.ProductId;
      ViewBag.ProductName = product.NameProduct;
      ViewBag.Search = search;
      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View(variants);
    }

    [HttpGet]
    public IActionResult CreateVariant(int productId)
    {
      ViewBag.ProductId = productId;
      ViewBag.ProductName = _context.Products
          .Where(p => p.ProductId == productId)
          .Select(p => p.NameProduct)
          .FirstOrDefault();

      ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale");
      ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName");


      return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVariant(int productId, ProductVariantCreateVM vm)
    {
      if (!ModelState.IsValid)
      {
        LoadDropdownsForVariant();
        ViewBag.ProductId = productId;
        ViewBag.ProductName = await _context.Products
            .Where(p => p.ProductId == productId)
            .Select(p => p.NameProduct)
            .FirstOrDefaultAsync();
        return View(vm);
      }

      using var tran = await _context.Database.BeginTransactionAsync();
      var uploadedFiles = new List<string>();

      try
      {
        // 1️⃣ Check Product tồn tại
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
          TempData["Error"] = "Sản phẩm không tồn tại";
          return RedirectToAction("Index");
        }

        // 2️⃣ Check trùng Variant (Product + Size + Color)
        bool isDuplicate = await _context.ProductVariants.AnyAsync(v =>
            v.ProductId == productId &&
            v.SizeId == vm.SizeId &&
            v.ColorId == vm.ColorId);

        if (isDuplicate)
        {
          ModelState.AddModelError("", "Biến thể (Size + Màu) đã tồn tại.");
          LoadDropdownsForVariant();
          ViewBag.ProductId = productId;
          ViewBag.ProductName = product.NameProduct;
          return View(vm);
        }

        // 3️⃣ Tạo Variant
        var variant = new ProductVariant
        {
          ProductId = productId,
          SizeId = vm.SizeId,
          ColorId = vm.ColorId,
          Price = vm.Price,
          Quantity = vm.Quantity,
          StatusVariant = vm.StatusVariant,
          CreatedAt = DateTime.Now
        };

        _context.ProductVariants.Add(variant);
        await _context.SaveChangesAsync(); // để lấy VariantId

        // 4️⃣ Upload Images (gắn với Variant)
        if (vm.Images != null && vm.Images.Any())
        {
          var webRoot = _env.WebRootPath
              ?? throw new Exception("WebRootPath is missing");

          var relativeDir = Path.Combine(
              "img",
              "products",
              //productId.ToString(),
              variant.VariantId.ToString()
          );

          var uploadDir = Path.Combine(webRoot, relativeDir);
          if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

          int index = 0;
          var timePrefix = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

          foreach (var file in vm.Images)
          {
            if (file == null || file.Length == 0) continue;
            if (file.Length > 2 * 1024 * 1024) continue;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowExt.Contains(ext)) continue;

            string fileName = index == 0 ? $"{timePrefix}_main{ext}" : $"{timePrefix}_sub_{index}{ext}";

            var fullPath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            uploadedFiles.Add(fullPath);

            _context.ProductImages.Add(new ProductImage
            {
              VariantId = variant.VariantId,
              UrlImage = Path.Combine(relativeDir, fileName).Replace("\\", "/"),
              IsMain = (index == 0),
              CreateAt = DateTime.Now
            });

            index++;
          }

          await _context.SaveChangesAsync();
        }

        await tran.CommitAsync();

        TempData["Success"] = "Thêm biến thể thành công";
        return RedirectToAction("Variants", new { id = productId });
      }
      catch (Exception ex)
      {
        await tran.RollbackAsync();

        // rollback file
        foreach (var path in uploadedFiles)
        {
          if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        }

        TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
        LoadDropdownsForVariant();
        ViewBag.ProductId = productId;
        return View(vm);
      }
    }

    [HttpGet]
    public async Task<IActionResult> EditVariant(int id)
    {
      // id = VariantId
      var variant = await _context.ProductVariants
     .Include(v => v.Product)
     .Include(v => v.ProductImages)
     .FirstOrDefaultAsync(v => v.VariantId == id);

      if (variant == null)
        return NotFound();

      var vm = new ProductVariantEditVM
      {
        VariantId = variant.VariantId,
        ProductId = variant.ProductId,
        SizeId = variant.SizeId,
        ColorId = variant.ColorId,
        Price = variant.Price,
        Quantity = variant.Quantity,
        StatusVariant = variant.StatusVariant,

        ExistingImages = variant.ProductImages
          .Where(i => !string.IsNullOrEmpty(i.UrlImage))
          .OrderByDescending(i => i.IsMain)
          .Select(i => i.UrlImage.TrimStart('~'))
          .ToList()

      };

      ViewBag.ProductName = variant.Product?.NameProduct ?? "N/A";

      LoadDropdownsForVariant(vm.SizeId, vm.ColorId);

      return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditVariant(ProductVariantEditVM vm)
    {
      if (!ModelState.IsValid)
      {
        LoadDropdownsForVariant(vm.SizeId, vm.ColorId);

        ViewBag.ProductId = vm.ProductId;
        ViewBag.ProductName = await _context.Products
            .Where(p => p.ProductId == vm.ProductId)
            .Select(p => p.NameProduct)
            .FirstOrDefaultAsync();

        return View(vm);
      }

      using var tran = await _context.Database.BeginTransactionAsync();
      var deletedFiles = new List<string>();
      var uploadedFiles = new List<string>();

      try
      {
        // 1️⃣ Load Variant + Product + Images
        var variant = await _context.ProductVariants
            .Include(v => v.Product)
            .Include(v => v.ProductImages)
            .FirstOrDefaultAsync(v => v.VariantId == vm.VariantId);

        if (variant == null)
          return NotFound();

        // 2️⃣ Check trùng Size + Color (ngoại trừ chính nó)
        bool isDuplicate = await _context.ProductVariants.AnyAsync(v =>
            v.ProductId == variant.ProductId &&
            v.VariantId != variant.VariantId &&
            v.SizeId == vm.SizeId &&
            v.ColorId == vm.ColorId);

        if (isDuplicate)
        {
          ModelState.AddModelError("", "Biến thể (Size + Màu) đã tồn tại.");
          LoadDropdownsForVariant(vm.SizeId, vm.ColorId);
          ViewBag.ProductId = variant.ProductId;
          ViewBag.ProductName = variant.Product.NameProduct;
          return View(vm);
        }

        // 3️⃣ Update Variant
        variant.SizeId = vm.SizeId;
        variant.ColorId = vm.ColorId;
        variant.Price = vm.Price;
        variant.Quantity = vm.Quantity;
        variant.StatusVariant = "ACTIVE";
        //variant.UpdatedAt = DateTime.Now;

        // 4️⃣ Nếu upload ảnh mới → thay toàn bộ ảnh cũ
        if (vm.Images != null && vm.Images.Any())
        {
          var webRoot = _env.WebRootPath
              ?? throw new Exception("WebRootPath is missing");

          var relativeDir = Path.Combine(
              "img",
              "products",
              //variant.ProductId.ToString(),
              variant.VariantId.ToString()
          );

          var uploadDir = Path.Combine(webRoot, relativeDir);
          if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

          // lưu path ảnh cũ để xóa sau commit
          deletedFiles = variant.ProductImages
              .Where(i => !string.IsNullOrEmpty(i.UrlImage))
              .Select(i => Path.Combine(webRoot, i.UrlImage!))
              .ToList();

          // xóa record ảnh cũ
          _context.ProductImages.RemoveRange(variant.ProductImages);

          int index = 0;
          var timePrefix = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

          foreach (var file in vm.Images)
          {
            if (file == null || file.Length == 0) continue;
            if (file.Length > 2 * 1024 * 1024) continue;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowExt.Contains(ext)) continue;

            string fileName = index == 0 ? $"{timePrefix}_main{ext}" : $"{timePrefix}_sub_{index}{ext}";

            var fullPath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            uploadedFiles.Add(fullPath);

            _context.ProductImages.Add(new ProductImage
            {
              VariantId = variant.VariantId,
              UrlImage = Path.Combine(relativeDir, fileName).Replace("\\", "/"),
              IsMain = (index == 0),
              CreateAt = DateTime.Now
            });

            index++;
          }
        }

        await _context.SaveChangesAsync();
        await tran.CommitAsync();

        // 5️⃣ Xóa file cũ sau commit
        foreach (var path in deletedFiles)
          if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);

        TempData["Success"] = "Cập nhật biến thể thành công";
        return RedirectToAction("Variants", new { id = variant.ProductId });
      }
      catch (Exception ex)
      {
        await tran.RollbackAsync();

        // rollback file mới
        foreach (var path in uploadedFiles)
          if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);

        TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;

        LoadDropdownsForVariant(vm.SizeId, vm.ColorId);
        ViewBag.ProductId = vm.ProductId;
        return View(vm);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVariant(int id)
    {
      var variant = await _context.ProductVariants
          .Include(v => v.Product)
          .FirstOrDefaultAsync(v => v.VariantId == id);

      if (variant == null)
        return NotFound();

      // 1️⃣ Soft delete Variant
      variant.StatusVariant = "INACTIVE";
      //variant.UpdatedAt = DateTime.Now;

      // 2️⃣ Nếu không còn Variant ACTIVE → Product cũng INACTIVE
      bool hasOtherActive = await _context.ProductVariants
          .AnyAsync(v => v.ProductId == variant.ProductId
                      && v.VariantId != id
                      && v.StatusVariant == "ACTIVE");

      if (!hasOtherActive && variant.Product != null)
      {
        variant.Product.StatusProduct = "INACTIVE";
        variant.Product.UpdateAt = DateTime.Now;
      }

      await _context.SaveChangesAsync();

      TempData["Success"] = "Đã xóa biến thể thành công.";
      return RedirectToAction("Variants", new { id = variant.ProductId });
    }


    [HttpGet]
    public IActionResult Create()
    {
      LoadDropdownsForProduct();
      return View();
    }

    //------Tri Trong Trang
    //Tao chuc nang them san pham
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
      if (!ModelState.IsValid)
      {
        LoadDropdownsForProduct();
        return View(vm);
      }

      using var tran = await _context.Database.BeginTransactionAsync();
      try
      {
        var productName = vm.NameProduct.Trim();

        // Optional: check trùng tên
        bool exists = await _context.Products
            .AnyAsync(p => p.NameProduct.ToLower() == productName.ToLower());

        if (exists)
        {
          ModelState.AddModelError("NameProduct", "Tên sản phẩm đã tồn tại");
          LoadDropdownsForProduct();
          return View(vm);
        }

        var product = new Product
        {
          NameProduct = productName,
          Descriptions = vm.Descriptions,
          CategoryId = vm.CategoryId,
          SupplierId = vm.SupplierId,
          TrademarkId = vm.TrademarkId,
          PromotionId = vm.PromotionId,
          StatusProduct = vm.StatusProduct,
          CreateAt = DateTime.Now
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await tran.CommitAsync();

        TempData["Success"] = "Tạo sản phẩm thành công. Vui lòng thêm biến thể.";
        return RedirectToAction("Variants", new { id = product.ProductId });
      }
      catch (Exception ex)
      {
        await tran.RollbackAsync();
        TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
        LoadDropdownsForProduct();
        return View(vm);
      }
    }
    //------


    //------Tri Trong Trang
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
      var product = await _context.Products
          .FirstOrDefaultAsync(p => p.ProductId == id);

      if (product == null)
      {
        TempData["Error"] = "Sản phẩm không tồn tại";
        return RedirectToAction("Index");
      }

      var vm = new ProductEditVM
      {
        ProductId = product.ProductId,
        NameProduct = product.NameProduct,
        Descriptions = product.Descriptions,
        CategoryId = product.CategoryId,
        SupplierId = product.SupplierId,
        TrademarkId = product.TrademarkId,
        PromotionId = product.PromotionId,
        StatusProduct = product.StatusProduct
      };

      LoadDropdownsForProduct(); // dùng lại cho Edit
      return View(vm);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditVM model)
    {
      if (!ModelState.IsValid)
      {
        LoadDropdownsForEdit(model);
        return View(model);
      }

      using var tran = await _context.Database.BeginTransactionAsync();

      try
      {
        bool isDuplicate = await _context.Products.AnyAsync(p =>
            p.ProductId != model.ProductId &&
            p.NameProduct.ToLower() == model.NameProduct.ToLower());

        if (isDuplicate)
        {
          ModelState.AddModelError("NameProduct", "Tên sản phẩm đã tồn tại");
          LoadDropdownsForEdit(model);
          return View(model);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

        if (product == null)
          return NotFound();

        // Update Product
        product.NameProduct = (model.NameProduct ?? "").Trim();
        product.Descriptions = model.Descriptions;
        product.CategoryId = model.CategoryId;
        product.SupplierId = model.SupplierId;
        product.TrademarkId = model.TrademarkId;
        product.PromotionId = model.PromotionId;
        product.StatusProduct = model.StatusProduct;
        product.UpdateAt = DateTime.Now;

        await _context.SaveChangesAsync();
        await tran.CommitAsync();

        TempData["Success"] = "Cập nhật sản phẩm thành công!";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        await tran.RollbackAsync();
        TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
        LoadDropdownsForEdit(model);
        return View(model);
      }
    }
    //------

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      // 1️⃣ Load Product + Variants
      var product = await _context.Products
          .Include(p => p.ProductVariants)
          .FirstOrDefaultAsync(p => p.ProductId == id);

      if (product == null)
        return NotFound();

      // 2️⃣ Soft delete Product
      product.StatusProduct = "INACTIVE";
      product.UpdateAt = DateTime.Now;

      // 3️⃣ Soft delete toàn bộ Variant con
      foreach (var variant in product.ProductVariants)
      {
        variant.StatusVariant = "INACTIVE";
        //variant.UpdateAt = DateTime.Now;
      }

      await _context.SaveChangesAsync();

      TempData["Success"] = "Sản phẩm đã được chuyển sang trạng thái không hoạt động.";
      return RedirectToAction(nameof(Index));
    }

    //------


    private void LoadDropdownsForProduct()
    {
      ViewBag.CategoryId = new SelectList(
          _context.Categories,
          "CategoryId",
          "NameCategory"
      );

      ViewBag.SupplierId = new SelectList(
          _context.Suppliers,
          "SupplierId",
          "NameSupplier"
      );

      ViewBag.TrademarkId = new SelectList(
          _context.Trademarks,
          "TrademarkId",
          "NameTrademark"
      );

      ViewBag.PromotionId = _context.Promotions
          .Where(p => p.StatusPromotion == "ACTIVE")
          .Select(p => new SelectListItem
          {
            Value = p.PromotionId.ToString(),
            Text = p.DiscountType == "%"
                ? $"{p.DiscountValue}%"
                : $"{p.DiscountValue:N0} VND"
          })
          .ToList();
    }


    private void LoadDropdownsForCreate(ProductCreateVM vm)
    {
      ViewBag.CategoryId = new SelectList(
       _context.Categories.AsNoTracking(),
       "CategoryId",
       "NameCategory",
       vm.CategoryId
   );

      ViewBag.SupplierId = new SelectList(
          _context.Suppliers.AsNoTracking(),
          "SupplierId",
          "NameSupplier",
          vm.SupplierId
      );

      ViewBag.TrademarkId = new SelectList(
          _context.Trademarks.AsNoTracking(),
          "TrademarkId",
          "NameTrademark",
          vm.TrademarkId
      );

      ViewBag.PromotionId = _context.Promotions
          .AsNoTracking()
          .Where(p => p.StatusPromotion == "ACTIVE")
          .Select(p => new SelectListItem
          {
            Value = p.PromotionId.ToString(),
            Text = p.DiscountType == "%"
                ? $"{p.DiscountValue}%"
                : $"{p.DiscountValue:N0} VND",
            Selected = vm.PromotionId == p.PromotionId
          })
          .ToList();

    }

    private void LoadDropdownsForEdit(ProductEditVM model)
    {
      ViewBag.CategoryId = new SelectList(
      _context.Categories.AsNoTracking(),
      "CategoryId",
      "NameCategory",
      model.CategoryId
  );

      ViewBag.SupplierId = new SelectList(
          _context.Suppliers.AsNoTracking(),
          "SupplierId",
          "NameSupplier",
          model.SupplierId
      );

      ViewBag.TrademarkId = new SelectList(
          _context.Trademarks.AsNoTracking(),
          "TrademarkId",
          "NameTrademark",
          model.TrademarkId
      );

      ViewBag.PromotionId = _context.Promotions
          .AsNoTracking()
          .Where(p => p.StatusPromotion == "ACTIVE" || p.PromotionId == model.PromotionId)
          .Select(p => new SelectListItem
          {
            Value = p.PromotionId.ToString(),
            Text = p.DiscountType == "%"
                ? $"{p.DiscountValue}%"
                : $"{p.DiscountValue:N0} VND",
            Selected = model.PromotionId == p.PromotionId
          })
          .ToList();
    }


    private void LoadDropdownsForVariant(int? selectedSizeId = null, int? selectedColorId = null)
    {
      ViewBag.SizeId = new SelectList(
          _context.Sizes.OrderBy(x => x.Scale),
          "SizeId",
          "Scale",
          selectedSizeId
      );

      ViewBag.ColorId = new SelectList(
          _context.Colors.OrderBy(x => x.ColorName),
          "ColorId",
          "ColorName",
          selectedColorId
      );
    }



    //public async Task<IActionResult> Index(string search, int page = 1)
    //{
    //  int pageSize = 10;

    //  var query = _context.ProductVariants
    //      .Include(v => v.Product)
    //        .ThenInclude(v => v.Category)
    //      .Include(v => v.Size)
    //      .Include(v => v.Color)
    //      .AsQueryable();
    //  // Tìm kiếm theo tên sản phẩm
    //  if (!string.IsNullOrEmpty(search))
    //  {
    //    query = query.Where(v => v.Product.NameProduct.Contains(search));
    //  }

    //  int totalItems = await query.CountAsync();
    //  int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

    //  var result = await query
    //      .OrderBy(v => v.VariantId)
    //      .Skip((page - 1) * pageSize)
    //      .Take(pageSize)
    //      .ToListAsync();

    //  ViewBag.Search = search;
    //  ViewBag.CurrentPage = page;
    //  ViewBag.TotalPages = totalPages;

    //  if (page < 1) page = 1;
    //  if (page > totalPages && totalPages > 0) page = totalPages;


    //  return View(result);
    //}



    //[HttpGet]
    //public IActionResult Create()
    //{
    //  LoadDropdownsForCreate();
    //  return View();
    //}
    ////------Tri Trong Trang
    ////Tao chuc nang them san pham
    //[HttpPost]
    //public async Task<IActionResult> Create(ProductCreateVM vm)
    //{
    //  if (!ModelState.IsValid)
    //  {
    //    LoadDropdownsForCreate(vm);
    //    return View(vm);
    //  }

    //  var uploadedFiles = new List<string>();
    //  using var tran = await _context.Database.BeginTransactionAsync();

    //  try
    //  {
    //    // 1. Xử lý Product (Đảm bảo productName không null)
    //    var productName = (vm.NameProduct ?? "").Trim();
    //    var product = await _context.Products
    //        .Include(p => p.Category)
    //        .FirstOrDefaultAsync(p => p.NameProduct.ToLower() == productName.ToLower());


    //    if (product == null)
    //    {
    //      product = new Product
    //      {
    //        NameProduct = productName,
    //        Descriptions = vm.Descriptions,
    //        CategoryId = vm.CategoryId,
    //        SupplierId = vm.SupplierId,
    //        TrademarkId = vm.TrademarkId,
    //        PromotionId = vm.PromotionId,
    //        StatusProduct = vm.StatusProduct ?? "ACTIVE",
    //        CreateAt = DateTime.Now
    //      };
    //      _context.Products.Add(product);
    //      await _context.SaveChangesAsync();
    //    }

    //    // 2. Check trùng Variant
    //    var duplicateVariant = await _context.ProductVariants
    //        .AnyAsync(v => v.ProductId == product.ProductId
    //                     && v.SizeId == vm.SizeId
    //                     && v.ColorId == vm.ColorId);

    //    if (duplicateVariant)
    //    {
    //      ModelState.AddModelError("", "Biến thể (Size và Màu) này đã tồn tại.");
    //      LoadDropdownsForCreate(vm);
    //      return View(vm);
    //    }

    //    // 3. Khởi tạo Variant
    //    var variant = new ProductVariant
    //    {
    //      ProductId = product.ProductId,
    //      SizeId = vm.SizeId,
    //      ColorId = vm.ColorId,
    //      Price = vm.Price,
    //      Quantity = vm.Quantity,
    //      StatusVariant = vm.StatusProduct ?? "ACTIVE",
    //      CreatedAt = DateTime.Now
    //    };
    //    _context.ProductVariants.Add(variant);
    //    // Lưu Variant ngay để lấy VariantId cho bảng Image
    //    await _context.SaveChangesAsync();

    //    // 4. Xử lý Images
    //    if (vm.Images != null && vm.Images.Any())
    //    {
    //      var webRoot = _env.WebRootPath ?? throw new Exception("WebRootPath is missing");

    //      var categoryName = Slugify(product.Category!.NameCategory);
    //      var productNameSlug = Slugify(product.NameProduct);

    //      var relativeDir = Path.Combine(
    //          "Images",
    //          categoryName,
    //          productNameSlug
    //      );
    //      var uploadDir = Path.Combine(webRoot, relativeDir);
    //      if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

    //      int index = 0;
    //      foreach (var file in vm.Images)
    //      {
    //        if (file == null || file.Length == 0 || file.Length > 2 * 1024 * 1024) continue;

    //        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
    //        var allowExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
    //        if (!allowExt.Contains(ext)) continue;

    //        var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss_fff}_{Guid.NewGuid().ToString("N").Substring(0, 6)}{ext}";
    //        var fullPath = Path.Combine(uploadDir, fileName);

    //        using (var stream = new FileStream(fullPath, FileMode.Create))
    //        {
    //          await file.CopyToAsync(stream);
    //        }
    //        uploadedFiles.Add(fullPath);

    //        _context.ProductImages.Add(new ProductImage
    //        {
    //          VariantId = variant.VariantId,
    //          UrlImage = Path.Combine(relativeDir, fileName).Replace("\\", "/"),
    //          IsMain = (index == 0),
    //          CreateAt = DateTime.Now
    //        });
    //        index++;
    //      }
    //      await _context.SaveChangesAsync();
    //    }

    //    await tran.CommitAsync();
    //    TempData["Success"] = "Lưu sản phẩm và biến thể thành công!";
    //    return RedirectToAction("Index");
    //  }
    //  catch (Exception ex)
    //  {
    //    await tran.RollbackAsync();
    //    foreach (var filePath in uploadedFiles)
    //    {
    //      if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
    //    }
    //    TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
    //    LoadDropdownsForCreate(vm);
    //    return View(vm);
    //  }
    //}
    ////------


    ////------Tri Trong Trang
    //[HttpGet]
    //public async Task<IActionResult> Edit(int id)
    //{
    //  var variant = await _context.ProductVariants
    //          .Include(v => v.Product)
    //          .Include(v => v.ProductImages)
    //          .FirstOrDefaultAsync(v => v.VariantId == id);

    //  if (variant == null || variant.Product == null)
    //    return NotFound();

    //  var product = variant.Product;

    //  var vm = new ProductEditVM
    //  {
    //    ProductId = product.ProductId,
    //    NameProduct = product.NameProduct,
    //    Descriptions = product.Descriptions,
    //    StatusProduct = product.StatusProduct ?? "ACTIVE",
    //    CategoryId = product.CategoryId,
    //    TrademarkId = product.TrademarkId,
    //    SupplierId = product.SupplierId,
    //    PromotionId = product.PromotionId,

    //    Price = variant.Price,
    //    Quantity = variant.Quantity,
    //    SizeId = variant.SizeId,
    //    ColorId = variant.ColorId,
    //    ProductVariantId = variant.VariantId,

    //    OldImages = variant.ProductImages
    //      .Where(x => !string.IsNullOrEmpty(x.UrlImage))
    //      .Select(x => "/" + x.UrlImage!)
    //      .ToList()

    //  };

    //  LoadDropdownsForEdit(vm);
    //  return View(vm);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Edit(ProductEditVM model)
    //{
    //  if (!ModelState.IsValid)
    //  {
    //    LoadDropdownsForEdit(model);
    //    return View(model);
    //  }

    //  using var tran = await _context.Database.BeginTransactionAsync();
    //  var deletedFiles = new List<string>();

    //  try
    //  {
    //    var product = await _context.Products
    //        .Include(p => p.Category)
    //        .Include(p => p.ProductVariants)
    //            .ThenInclude(v => v.ProductImages)
    //        .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

    //    if (product == null) return NotFound();

    //    var variant = product.ProductVariants.FirstOrDefault(v => v.VariantId == model.ProductVariantId)
    //                  ?? product.ProductVariants.FirstOrDefault();

    //    if (variant == null) return NotFound("Không tìm thấy biến thể.");

    //    // Kiểm tra trùng Size/Color với biến thể KHÁC
    //    var isDuplicate = await _context.ProductVariants
    //        .AnyAsync(v => v.ProductId == product.ProductId
    //                    && v.VariantId != variant.VariantId
    //                    && v.SizeId == model.SizeId
    //                    && v.ColorId == model.ColorId);

    //    if (isDuplicate)
    //    {
    //      ModelState.AddModelError("", "Kích thước và màu sắc này đã tồn tại cho một biến thể khác.");
    //      LoadDropdownsForEdit(model);
    //      return View(model);
    //    }

    //    // Cập nhật thông tin
    //    product.NameProduct = model.NameProduct ?? "";
    //    product.Descriptions = model.Descriptions;
    //    product.CategoryId = model.CategoryId;
    //    product.TrademarkId = model.TrademarkId;
    //    product.SupplierId = model.SupplierId;
    //    product.PromotionId = model.PromotionId;
    //    product.StatusProduct = model.StatusProduct;
    //    product.UpdateAt = DateTime.Now;

    //    variant.Price = model.Price;
    //    variant.Quantity = model.Quantity;
    //    variant.SizeId = model.SizeId;
    //    variant.ColorId = model.ColorId;
    //    variant.StatusVariant = model.StatusProduct;

    //    // Xử lý ảnh
    //    if (model.Images != null && model.Images.Any())
    //    {
    //      var webRoot = _env.WebRootPath ?? "wwwroot";
    //      var categoryName = Slugify(product.Category!.NameCategory);
    //      var productNameSlug = Slugify(product.NameProduct);

    //      var relativeDir = Path.Combine(
    //          "Images",
    //          categoryName,
    //          productNameSlug
    //      );
    //      var uploadDir = Path.Combine(webRoot, relativeDir);

    //      if (!Directory.Exists(uploadDir))
    //        Directory.CreateDirectory(uploadDir);

    //      // Lọc và lưu path ảnh cũ
    //      var oldImagePaths = variant.ProductImages
    //          .Where(x => !string.IsNullOrEmpty(x.UrlImage))
    //          .Select(x => Path.Combine(webRoot, x.UrlImage!))
    //          .ToList();

    //      _context.ProductImages.RemoveRange(variant.ProductImages);

    //      int index = 0;
    //      foreach (var file in model.Images)
    //      {
    //        if (file == null || file.Length == 0) continue;

    //        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
    //        var allowExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
    //        if (!allowExt.Contains(ext)) continue;
    //        var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss_fff}_{Guid.NewGuid().ToString("N").Substring(0, 6)}{ext}";
    //        var fullPath = Path.Combine(uploadDir, fileName);

    //        using (var stream = new FileStream(fullPath, FileMode.Create))
    //        {
    //          await file.CopyToAsync(stream);
    //        }

    //        _context.ProductImages.Add(new ProductImage
    //        {
    //          VariantId = variant.VariantId,
    //          UrlImage = Path.Combine(relativeDir, fileName).Replace("\\", "/"),
    //          IsMain = (index == 0),
    //          CreateAt = DateTime.Now
    //        });
    //        index++;
    //      }
    //      deletedFiles.AddRange(oldImagePaths);
    //    }

    //    await _context.SaveChangesAsync();
    //    await tran.CommitAsync();

    //    // Xóa file vật lý sau commit thành công
    //    foreach (var oldPath in deletedFiles)
    //    {
    //      if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
    //    }

    //    TempData["Success"] = "Cập nhật thành công!";
    //    return RedirectToAction("Index");
    //  }
    //  catch (Exception ex)
    //  {
    //    await tran.RollbackAsync();
    //    TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
    //    LoadDropdownsForEdit(model); // Đảm bảo dropdown không bị mất dữ liệu
    //    return View(model);
    //  }
    //}
    ////------

    ////------Tri Trong Trang
    ////Tao chuc nang xoa san pham (cap nhat trang thai)
    //[HttpPost]
    //public async Task<IActionResult> Delete(int id)
    //{
    //  // 1. Dùng Include để lấy luôn Product cha, giảm 1 lần FindAsync sau này
    //  var variant = await _context.ProductVariants
    //      .Include(v => v.Product)
    //      .FirstOrDefaultAsync(v => v.VariantId == id);

    //  if (variant == null) return NotFound();

    //  // 2. Cập nhật trạng thái biến thể
    //  variant.StatusVariant = "INACTIVE";

    //  // 3. Tối ưu: Kiểm tra các biến thể còn lại ngay trên Product đã được load
    //  // Cách này không cần truy vấn DB thêm 1 lần AnyAsync nữa
    //  if (variant.Product != null)
    //  {
    //    // Kiểm tra xem có anh em nào khác của biến thể này còn ACTIVE không
    //    bool hasOtherActive = await _context.ProductVariants
    //        .AnyAsync(v => v.ProductId == variant.ProductId
    //                    && v.VariantId != id // Loại trừ chính nó
    //                    && v.StatusVariant == "ACTIVE");

    //    if (!hasOtherActive)
    //    {
    //      variant.Product.StatusProduct = "INACTIVE";
    //      variant.Product.UpdateAt = DateTime.Now; // Nên cập nhật ngày sửa
    //    }
    //  }

    //  await _context.SaveChangesAsync();

    //  TempData["Success"] = "Đã chuyển trạng thái sang không hoạt động.";
    //  return RedirectToAction(nameof(Index));
    //}
    ////------



    //private void LoadDropdownsForCreate()
    //{
    //  ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory");
    //  ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier");
    //  ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark");

    //  ViewBag.PromotionId = _context.Promotions
    //      .Select(p => new SelectListItem
    //      {
    //        Value = p.PromotionId.ToString(),
    //        Text = p.DiscountType == "%"
    //            ? $"{p.DiscountValue}%"
    //            : $"{p.DiscountValue:N0} VND"
    //      })
    //      .ToList();

    //  ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale");
    //  ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName");
    //}

    //private void LoadDropdownsForCreate(ProductCreateVM vm)
    //{
    //  ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory", vm.CategoryId);
    //  ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier", vm.SupplierId);
    //  ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark", vm.TrademarkId);

    //  ViewBag.PromotionId = _context.Promotions
    //      .Select(p => new SelectListItem
    //      {
    //        Value = p.PromotionId.ToString(),
    //        Text = p.DiscountType == "%"
    //            ? $"{p.DiscountValue}%"
    //            : $"{p.DiscountValue:N0} VND",
    //        Selected = vm.PromotionId == p.PromotionId
    //      })
    //      .ToList();

    //  ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale", vm.SizeId);
    //  ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName", vm.ColorId);
    //}

    //private void LoadDropdownsForEdit(ProductEditVM model)
    //{
    //  ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory", model.CategoryId);
    //  ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier", model.SupplierId);
    //  ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark", model.TrademarkId);

    //  ViewBag.PromotionId = _context.Promotions
    //      .Select(p => new SelectListItem
    //      {
    //        Value = p.PromotionId.ToString(),
    //        Text = p.DiscountType == "%"
    //            ? $"{p.DiscountValue}%"
    //            : $"{p.DiscountValue:N0} VND",
    //        Selected = model.PromotionId == p.PromotionId
    //      })
    //      .ToList();

    //  ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale", model.SizeId);
    //  ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName", model.ColorId);
    //}

    //private static string Slugify(string input)
    //{
    //  if (string.IsNullOrWhiteSpace(input)) return "unknown";

    //  foreach (var c in Path.GetInvalidFileNameChars())
    //  {
    //    input = input.Replace(c, '-');
    //  }
    //  return input.Trim();
    //}



  }


}
