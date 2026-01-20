using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
  public class AboutController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
