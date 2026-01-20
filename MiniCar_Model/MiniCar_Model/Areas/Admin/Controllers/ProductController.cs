using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;
using System.Drawing;

namespace MiniCar_Model.Areas.Admin.Controllers
{
  [Area("Admin")]
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

      var query = _context.ProductVariants
          .Include(v => v.Product)
            .ThenInclude(v => v.Category)
          .Include(v => v.Size)
          .Include(v => v.Color)
          .AsQueryable();
      // Tìm kiếm theo tên sản phẩm
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(v => v.Product.NameProduct.Contains(search));
      }

      int totalItems = await query.CountAsync();
      int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

      var result = await query
          .OrderBy(v => v.VariantId)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      ViewBag.Search = search;
      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = totalPages;

      if (page < 1) page = 1;
      if (page > totalPages && totalPages > 0) page = totalPages;


      return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory");
      ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier");
      ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark");
      //ViewBag.PromotionId = new SelectList(_context.Promotions, "PromotionId", "DiscountValue");
      var promos = _context.Promotions
        .Select(p => new
        {
          p.PromotionId,
          p.DiscountType,
          p.DiscountValue
        })
        .ToList();

      ViewBag.PromotionId = promos
          .Select(x => new SelectListItem
          {
            Value = x.PromotionId.ToString(),
            Text = x.DiscountType == "%"
                     ? $"{x.DiscountValue}%"
                     : $"{x.DiscountValue:N0} VND"
          })
          .ToList();


      ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale");
      ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName");
      return View();
    }
    //------Tri Trong Trang
    //Tao chuc nang them san pham
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory");
        ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier");
        ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark");
        var promos = _context.Promotions
        .Select(p => new
        {
          p.PromotionId,
          p.DiscountType,
          p.DiscountValue
        })
        .ToList();

        ViewBag.PromotionId = promos.Select(x => new SelectListItem
        {
          Value = x.PromotionId.ToString(),
          Text = x.DiscountType == "%"
                ? $"{x.DiscountValue}%"
                : $"{x.DiscountValue:N0} VND"
        }).ToList();
        ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale");
        ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName");
        return View(vm);
      }

      using var tran = await _context.Database.BeginTransactionAsync();

      try
      {
        // 1. Tạo Product
        var product = new Product
        {
          NameProduct = vm.NameProduct,
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

        // 2. Tạo Variant
        var variant = new ProductVariant
        {
          ProductId = product.ProductId,
          SizeId = vm.SizeId,
          ColorId = vm.ColorId,
          Price = vm.Price,
          Quantity = vm.Quantity,
          StatusVariant = vm.StatusProduct,
          CreatedAt = DateTime.Now
        };

        _context.ProductVariants.Add(variant);
        await _context.SaveChangesAsync();

        // 3. Upload Images
        if (vm.Images != null && vm.Images.Any())
        {
          var uploadDir = Path.Combine(_env.WebRootPath, "product_images");

          if (!Directory.Exists(uploadDir))
          {
            Directory.CreateDirectory(uploadDir);
          }

          int index = 0;

          foreach (var file in vm.Images)
          {
            // 1️⃣ Check file null / rỗng / không tên
            if (file == null || file.Length == 0  || string.IsNullOrWhiteSpace(file.FileName))
              continue;

            // 3️⃣ Check extension
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowExt.Contains(ext))
              continue;

            // 4️⃣ Tạo tên file KHÔNG TRÙNG
            var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            var random = Guid.NewGuid().ToString("N").Substring(0, 6);
            var fileName = $"{timeStamp}_{random}{ext}";

            var path = Path.Combine(uploadDir, fileName);

            // 5️⃣ Lưu file
            using (var stream = new FileStream(path, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            // 6️⃣ Lưu DB
            _context.ProductImages.Add(new ProductImage
            {
              VariantId = variant.VariantId,
              UrlImage = "/product_images/" + fileName,
              IsMain = index == 0
            });

            index++;
          }

          await _context.SaveChangesAsync();
        }

        await tran.CommitAsync();
        TempData["Success"] = "Thêm sản phẩm thành công!";
        return RedirectToAction("Index");
      }
      catch
      {
        await tran.RollbackAsync();
        TempData["Error"] = "Lỗi khi lưu sản phẩm!";
        return View(vm);
      }
    }
    //------


    //------Tri Trong Trang
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
      var product = await _context.Products
        .Include(p => p.ProductVariants)
            .ThenInclude(v => v.ProductImages)
        .FirstOrDefaultAsync(x => x.ProductId == id);

      if (product == null)
        return NotFound();

      // Lấy 1 biến thể mặc định (hoặc bạn tự chọn)
      var variant = product.ProductVariants.FirstOrDefault();

      var vm = new ProductEditVM
      {
        ProductId = product.ProductId,
        NameProduct = product.NameProduct,
        Price = variant?.Price ?? 0,
        Quantity = variant?.Quantity ?? 0,
        Descriptions = product.Descriptions,
        StatusProduct = product.StatusProduct ?? "Active",

        CategoryId = product.CategoryId,
        TrademarkId = product.TrademarkId,
        SupplierId = product.SupplierId,
        PromotionId = product.PromotionId,

        SizeId = variant?.SizeId ?? 0,
        ColorId = variant?.ColorId ?? 0,

        OldImages = variant?.ProductImages?
                      .Select(x => "/uploads/" + x.UrlImage)
                      .ToList()
            ?? new List<string>()
      };

      // Load dropdown
        ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "NameCategory");
        ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "NameSupplier");
        ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "NameTrademark");
        ViewBag.PromotionId = new SelectList(_context.Promotions, "PromotionId", "DiscountValue");
        ViewBag.SizeId = new SelectList(_context.Sizes, "SizeId", "Scale");
        ViewBag.ColorId = new SelectList(_context.Colors, "ColorId", "ColorName");

      return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductEditVM model)
    {
      var product = await _context.Products
        .Include(p => p.ProductVariants)
            .ThenInclude(v => v.ProductImages)
        .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

      if (product == null)
        return NotFound();

      // Update Product
      product.NameProduct = model.NameProduct;
      product.Descriptions = model.Descriptions;
      product.CategoryId = model.CategoryId;
      product.TrademarkId = model.TrademarkId;
      product.SupplierId = model.SupplierId;
      product.PromotionId = model.PromotionId;
      product.StatusProduct = model.StatusProduct;
      product.UpdateAt = DateTime.Now;

      // Update Variant
      var variant = product.ProductVariants.FirstOrDefault();
      if (variant != null)
      {
        variant.Price = model.Price;
        variant.Quantity = model.Quantity;
        variant.SizeId = model.SizeId;
        variant.ColorId = model.ColorId;
        variant.StatusVariant = model.StatusProduct;
      }

      // Upload ảnh mới
      if (model.Images != null && model.Images.Count > 0)
      {
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(savePath))
          Directory.CreateDirectory(savePath);

        foreach (var file in model.Images)
        {
          var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
          var path = Path.Combine(savePath, filename);

          using (var stream = new FileStream(path, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }

          // Lưu ProductImage mới
          product.ProductVariants.First().ProductImages.Add(new ProductImage
          {
            UrlImage = filename,
            IsMain = false,
            CreateAt = DateTime.Now
          });
        }
      }

      await _context.SaveChangesAsync();
      return RedirectToAction("Index");
    }
    //------

    //------Tri Trong Trang
    //Tao chuc nang xoa san pham (cap nhat trang thai)
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      var variant = await _context.ProductVariants.FindAsync(id);
      if (variant == null)
        return NotFound();

      variant.StatusVariant = "INACTIVE";

      bool stillActiveVariants = await _context.ProductVariants
                                        .AnyAsync(x => x.ProductId == variant.ProductId && x.StatusVariant == "ACTIVE");

      if (!stillActiveVariants)
      {
        var product = await _context.Products.FindAsync(variant.ProductId);
        if (product != null)
        {
          product.StatusProduct = "INACTIVE";
        }
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }
    //------
  }


}
