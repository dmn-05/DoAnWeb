using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Controllers {
  public class AccountController : Controller {
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login() {
			var count = _context.Accounts.Count();
			ViewBag.TestDb = count;
			return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
      var account = _context.Accounts.FirstOrDefault(x =>
          (x.Email == email || x.UserName == email) &&
          x.PasswordAccount == password &&
          x.StatusAccount == "ACTIVE"
      );

      if (account == null)
      {
        ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
        return View();
      }

      // Lưu session
      HttpContext.Session.SetInt32("AccountId", account.AccountId);
      HttpContext.Session.SetInt32("RoleId", account.RoleId);
      HttpContext.Session.SetString("UserName", account.UserName);
      HttpContext.Session.SetString("FullName", account.NameAccount ?? "");

      return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult Register() {
      return View();
    }

    public IActionResult ForgotPassword() {
      return View();
    }

    public IActionResult Profile() {
      return View();
    }

    public IActionResult Logout() {
      return RedirectToAction("Index", "Home");
    }

    public IActionResult Order() {
      return View();
    }

    public IActionResult Wishlist() {
      return View();
    }

    public ActionResult EditAccount() {
      return View();
    }

    public IActionResult ChangePassword() {
      return View();
    }
  }
}
