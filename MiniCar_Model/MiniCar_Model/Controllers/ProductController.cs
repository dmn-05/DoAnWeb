using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers {
  public class ProductController : Controller {
    // GET: /Product/List
    [HttpGet]
    public IActionResult List() {
      return View();
    }

    // GET: /Product/Detail/:id
    [HttpGet]
    public IActionResult Detail() {
      return View();
    }
  }
}
