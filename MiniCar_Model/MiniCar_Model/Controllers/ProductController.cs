using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;

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
    public IActionResult Detail() {
      return View();
    }
  }
}
