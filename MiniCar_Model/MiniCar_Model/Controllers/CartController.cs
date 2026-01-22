using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers {
  public class CartController : Controller {
    // GET: /Cart
    [HttpGet]
    public IActionResult Index() {
      return View();
    }
    //thuong code 
    public IActionResult CheckLogin()
    {
      var accountId = HttpContext.Session.GetInt32("AccountId");

      return Json(new
      {
        isLogin = accountId != null
      });
    }
    //thuong end code
  }
}
