using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace MiniCar_Model.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //------Tri Trong Trang
        //private readonly ApplicationDbContext _context;
        //public ProductController(ApplicationBuilderDbcontext context)
        //{
        //    _context = context;
        //}
        //------


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }    
        public IActionResult Create()
        {
            return View();
        }
        //------Tri Trong Trang
        //Tao chuc nang them san pham
        //[HttpPost]
        //public async Task<IActionResult> Create(Product model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return View(product);
        //    }

        //    var product = new Product
        //    {
        //        Id = model.Id,
        //        Name = model.Name,
        //        Price = model.Price,
        //        Size = model.Size,
        //        Color = model.Color,
        //        Status = model.Status,
        //    };

        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    if (model.Images != null && model.Images.Count > 0)
        //    {
        //        var filename = $""
        //    }

        //    return View();
        //}
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
