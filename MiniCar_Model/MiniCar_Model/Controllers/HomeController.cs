using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;

namespace MiniCar_Model.Controllers {
  public class HomeController : Controller {
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context) {
      _context = context;
    }

    public IActionResult Index() {
      var latestProducts = _context.Products
          .OrderByDescending(p => p.CreateAt)
          .Take(8)
          .ToList();

      var discountProducts = _context.Products
          .Where(p => p.PromotionId != null)
          .Take(8)
          .ToList();

      var model = new HomeVM {
        LatestProducts = latestProducts,
        DiscountProducts = discountProducts
      };

      return View(model);
    }

    public IActionResult Contact() {
      return View();
    }

    public IActionResult Introduction() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
