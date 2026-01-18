using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;

namespace MiniCar_Model.Controllers {
  public class AccountController : Controller {
    private readonly ApplicationDbContext _context;
    //thuong code
    private readonly ILogger<AccountController> _logger;
    //thuong end code

    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger) {
      _context = context;
      //thuong code
      _logger = logger;
      //thuong end code
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
    public IActionResult Login(string email, string password) {
      var account = _context.Accounts.FirstOrDefault(x =>
          (x.Email == email || x.UserName == email) &&
          x.PasswordAccount == password &&
          x.StatusAccount == "ACTIVE"
      );

      if (account == null) {
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
      var accountId = HttpContext.Session.GetInt32("AccountId");

      if (accountId == null) {
        return RedirectToAction("Login");
      }

      var account = _context.Accounts
          .Include(x => x.Role)
          .FirstOrDefault(x => x.AccountId == accountId);
      return View(account);
    }
    //thuong code
    [HttpGet]
    public IActionResult EditProfile()
    {
      var accountId = HttpContext.Session.GetInt32("AccountId");

      var model = _context.Accounts
          .Where(a => a.AccountId == accountId)
          .Select(a => new Account
          {
            NameAccount = a.NameAccount,
            Email = a.Email,
            PhoneNumber = a.PhoneNumber,
            AddressAccount = a.AddressAccount
          })
          .FirstOrDefault();

      if (model == null)
        return NotFound();

      return View(model);
    }
    //thuong end code
    //thuong code
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProfile(Account model)
    {
      try
      {
        var accountId = HttpContext.Session.GetInt32("AccountId");

        var account = _context.Accounts
            .FirstOrDefault(a => a.AccountId == accountId);

        if (account == null)
          return NotFound();

        // Gán dữ liệu mới
        account.NameAccount = model.NameAccount;
        account.Email = model.Email;
        account.PhoneNumber = model.PhoneNumber;
        account.AddressAccount = model.AddressAccount;

        _context.SaveChanges();

        return RedirectToAction("Profile");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Lỗi cập nhật profile");
        return View(model);
      }
    }
    //thuong end code
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
