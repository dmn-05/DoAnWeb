using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using System.Drawing;

namespace MiniCar_Model.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class ProductController : Controller
  {
    //------Tri Trong Trang
    private readonly ApplicationDbContext _context;
    public ProductController(ApplicationDbContext context)
    {
      _context = context;
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
                        .OrderBy(v => v.VariantId)                       // sắp xếp ổn định
                        .Skip((page - 1) * pageSize)                    // bỏ các trang trước
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
      return View();
    }
    //------Tri Trong Trang
    //Tao chuc nang them san pham
    [HttpPost]
    public async Task<IActionResult> Create(Product model)
    {
      
      return View();
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
