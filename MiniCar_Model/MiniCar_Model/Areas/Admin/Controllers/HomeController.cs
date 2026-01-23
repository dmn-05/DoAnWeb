using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Filters;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;


namespace MiniCar_Model.Areas.Admin.Controllers
{
  [Area("Admin")]
  [AdminAuthorize]
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      var model = new DashboardViewModel
      {
        TotalAccounts = _context.Accounts.Count(),

        TotalProducts = _context.ProductVariants
              .Where(p => p.StatusVariant == "ACTIVE")
              .Select(p => p.ProductId)
              .Distinct()
              .Count(),

        TotalBills = _context.Bills.Count(),

        TotalSuppliers = _context.Suppliers.Count()
      };
      return View(model);
    }
  }
}
