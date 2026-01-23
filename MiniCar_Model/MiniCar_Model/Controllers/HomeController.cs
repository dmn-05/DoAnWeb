
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
          .Select(p => new ProductCardVM {
            ProductId = p.ProductId,
            NameProduct = p.NameProduct,

            NameCategory = p.Category.NameCategory,

            VariantId = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.VariantId)
                  .FirstOrDefault(),

            Price = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.Price)
                  .FirstOrDefault(),

            ImageUrl = p.ProductVariants
                  .SelectMany(v => v.ProductImages)
                  .OrderByDescending(i => i.IsMain)
                  .Select(i => i.UrlImage)
                  .FirstOrDefault()
          })
          .ToList();

      var discountProducts = _context.Products
          .Where(p => p.PromotionId != null)
          .Take(8)
          .Select(p => new ProductCardVM {
            ProductId = p.ProductId,
            NameProduct = p.NameProduct,

            NameCategory = p.Category.NameCategory,

            VariantId = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.VariantId)
                  .FirstOrDefault(),

            Price = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.Price)
                  .FirstOrDefault(),

            ImageUrl = p.ProductVariants
                  .SelectMany(v => v.ProductImages)
                  .OrderByDescending(i => i.IsMain)
                  .Select(i => i.UrlImage)
                  .FirstOrDefault()
          })
          .ToList();



      var banner = _context.Slideshows
        .Where(s => (s.StartDate <= DateTime.Now || s.EndDate > DateTime.Now) || s.StatusSlideshow == "Only")
        .ToList();
      var model = new HomeVM {
        LatestProducts = latestProducts,
        DiscountProducts = discountProducts,
        Slideshows = banner
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
    public async Task<IActionResult> Contact(ContactCreateVM model)
    {
      if (!ModelState.IsValid)
        return View(model);

      var contact = new Contact
      {
        Name = model.Name,
        Email = model.Email,
        Phone = model.Phone,
        Subject = model.Subject,
        Message = model.Message,

        CreatedAt = DateTime.Now,
        StatusContact = "Pending",
        IsDeleted = false,
        DeletedAt = null
      };

      _context.Contacts.Add(contact);
      await _context.SaveChangesAsync();

      ViewBag.Success = "Cảm ơn bạn đã liên hệ với chúng tôi";
      ModelState.Clear();

      return View();
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
