using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers {
  public class CartController : Controller {
    // GET: /Cart
    [HttpGet]
    public IActionResult Index() {
      return View();
    }
  }
}
