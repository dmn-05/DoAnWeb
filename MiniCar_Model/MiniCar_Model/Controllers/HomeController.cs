using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;
using System.Diagnostics;

namespace MiniCar_Model.Controllers
{

  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
      _logger = logger;
      _context = context;
    }

    public IActionResult Index()
    {
      var latestProducts = _context.Products
          .OrderByDescending(p => p.CreateAt)
          .Take(8)
          .ToList();

      var discountProducts = _context.Products
          .Where(p => p.PromotionId != null)
          .Take(8)
          .ToList();

      var model = new HomeVM
      {
        LatestProducts = latestProducts,
        DiscountProducts = discountProducts
      };
      Console.WriteLine(_context.Database.CanConnect());

      return View(model);
    }

    //------Tri Trong Treo
    //Xu ly trang lien he tu khach hang
    [HttpGet]
    public IActionResult Contact()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contact(Contact contact)
    {
      if (ModelState.IsValid)
      {
        contact.CreatedAt = DateTime.Now;
        contact.StatusContact = "New";

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        ViewBag.Success = "Cảm ơn bạn đã liên hệ với chúng tôi";
        ModelState.Clear();
        return View();
      }
      return View(contact);
    }
    //------Tri Trong Treo

    public IActionResult Introduction()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }



  }
}
