using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers {
  public class AccountController : Controller {
    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login() {
      return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public IActionResult Login(string username, string password) {
      //Xử lý login ở đây
      if(username == "user" && password == "123") {
        return RedirectToAction("Index", "Home");
      }

      ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
      return View();
    }

    [HttpGet]
    public IActionResult Register() {
      return View();
    }
  }
}
