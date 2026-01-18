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


    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
      if (page <  1) page = 1;
      var TotalVariants = await _context.ProductVariants.CountAsync();

      var variants = await _context.ProductVariants
                        .Include(v => v.Product)
                        .Include(v => v.Size)
                        .Include(v => v.Color)
                        .AsNoTracking()
                        .OrderBy(v => v.VariantId)                       
                        .Skip((page - 1) * pageSize)                    
                        .Take(pageSize)
                        .ToListAsync();

      ViewBag.CurrentPage = page;
      var totalPages = (int)Math.Ceiling((double)TotalVariants / pageSize);
      ViewBag.TotalPages = totalPages == 0 ? 1 : totalPages;
      return View(variants);
    }
    [HttpGet]
    public IActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
      ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
      ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "TrademarkName");
      ViewBag.PromotionId = new SelectList(_context.Promotions, "PromotionId", "PromotionName");
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
        ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
        ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
        ViewBag.TrademarkId = new SelectList(_context.Trademarks, "TrademarkId", "TrademarkName");
        ViewBag.PromotionId = new SelectList(_context.Promotions, "PromotionId", "PromotionName");
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
          StatusVariant = "Active",
          CreatedAt = DateTime.Now
        };

        _context.ProductVariants.Add(variant);
        await _context.SaveChangesAsync();

        // 3. Upload Images
        if (vm.Images != null && vm.Images.Any())
        {
          int index = 0;
          foreach (var file in vm.Images)
          {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string uploadPath = Path.Combine(_env.WebRootPath, "product_images", fileName);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            var img = new ProductImage
            {
              VariantId = variant.VariantId,
              UrlImage = "/product_images/" + fileName,
              IsMain = index == 0
            };

            _context.ProductImages.Add(img);
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
    //Tim kiem san pham theo ten
    //public async Task<IActionResult> Search(string nameproduct)
    //{
    //    var query = _context.Products.AsQueryable();

    //    if (!string.IsNullOrEmpty(nameproduct)) 
    //    {
    //        query = query.Where(p => p.Name.Contains(nameproduct));
    //    }
    //    ViewBag.Name = nameproduct;
    //    var result = await query.ToListAsync();
    //    return View(result);
    //}
    //------

    //------Tri Trong Trang
    //[HttpGet]
    //public async Task<IActionResult> Edit(int id)
    //{
    //    var product = await _context.Products.FindAsync(id);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(product);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Edit(Product model, int id, List<IFormFile> Images)
    //{
    //    var product = _context.Products.Find(id);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    product.Id = model.id;
    //    product.Name = model.Name;
    //    product.Price = model.Price;
    //    product.Size = model.Size;
    //    product.Color = model.Color;
    //    product.Status = model.Status;
    //    product.CategoryId = model.CategoryId;

    //    if (Images != null && Images.Count > 0)
    //    {
    //        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

    //        if (!Directory.Exists(savePath))
    //        {
    //            Directory.CreateDirectory(savePath);
    //        }

    //        var filenames = new List<string>();

    //        foreach (var file in Images)
    //        {
    //            var filename = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
    //            var path = Path.Combine(savePath, filename);

    //            using (var stream = new FileStream(path, FileMode.Create))
    //            {
    //                await file.CopyToAsync(stream);
    //            }

    //            product.ImagePaths = string.Join(";", filenames);

    //        }
    //    }

    //    await _context.SaveChangesAsync();
    //    return RedirectToAction("Index");
    //}
    //------

    //------Tri Trong Trang
    //Tao chuc nang xoa san pham (cap nhat trang thai)
    //[HttpPost]
    //public async IActionResult Delete(int id)
    //{
    //    var product = _context.Products.FirstOrDefault(x => x.Id == id);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    product.Status = "Inactive";

    //    await _context.SaveChanges();

    //    TempData["success"] = "Đã xóa sản phẩm";
    //    return RedirectToAction(nameof(Index));
    //}
    //------
  }


}
