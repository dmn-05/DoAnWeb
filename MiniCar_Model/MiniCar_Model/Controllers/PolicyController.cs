using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers {
  public class PolicyController : Controller {
    public IActionResult Shipping() {
      return View();
    }
    public IActionResult ReturnPolicy() {
      return View();
    }
    public IActionResult Support() {
      return View();
    }
    public IActionResult Payment() {
      return View();
    }
  }
}
