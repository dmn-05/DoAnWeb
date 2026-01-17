using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models.ViewModels;

namespace MiniCar_Model.Controllers {
  public class ProductController : Controller {

    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context) {
      _context = context;
    }

    // GET: /Product/List
    [HttpGet]
    public async Task<IActionResult> List(int page = 1) {
      int pageSize = 9;
      var totalProducts = await _context.Products.CountAsync();

      var products = await _context.Products
        .OrderBy(p => p.ProductId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
      return View(products);
    }

    // GET: /Product/Detail/:id
    [HttpGet]
    public async Task<IActionResult> Detail(int id) {

      var product = await _context.Products
                      .Include(x => x.Category)
                      .FirstOrDefaultAsync(x => x.ProductId == id);

      if (product == null)
      {
        return NotFound();
      }

      var variants = await _context.ProductVariants
                      .Where(v => v.ProductId == id)
                      .Include(v => v.Size)
                      .Include(v => v.Color)
                      .Include(v => v.ProductImages)
                      .Include(v => v.Comments)
                      .ToListAsync();

      var images = variants.SelectMany(v => v.ProductImages).ToList();

      var comments = variants.SelectMany(v => v.Comments).OrderByDescending(c => c.CreateAt).ToList();

      var avgRating = comments.Any() ? comments.Average(c => c.Rating).GetValueOrDefault() : 0;

      var firstvariant = variants.FirstOrDefault();


      var vm = new ProductDetailVM
      {
        Product = product,
        Variants = variants,
        Images = images,
        SelectedVariantId = firstvariant?.VariantId ?? 0,
        Price = firstvariant?.Price ?? 0,
        Quanlity = firstvariant?.Quantity ?? 0,
        Comments = comments,
        AvgRating = avgRating
      };


      return View(vm);
    }
  }
}
