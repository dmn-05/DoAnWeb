using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Areas.Admin.Controllers {
  [Area("Admin")]
  public class CartController : Controller {
    public IActionResult Index() {
      return View();
    }
  }
}
