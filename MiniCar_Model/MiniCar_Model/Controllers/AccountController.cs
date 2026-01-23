using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;


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
      //thuong code
      if (HttpContext.Session.GetInt32("AccountId") != null) {
        if (HttpContext.Session.GetInt32("RoleId") == 1)
          return RedirectToAction("Index", "Home", new { area = "Admin" });
        return RedirectToAction("Index", "Home");
      }
      //thuong end code
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
      //thuong code
      if (account.RoleId == 1)
        return RedirectToAction("Index", "Home", new { area = "Admin" });

      return RedirectToAction("Index", "Home");
      //thuong end code
    }
    [HttpPost]
    public IActionResult LoginPopup(string email, string password) {
      var account = _context.Accounts.FirstOrDefault(x =>
          (x.Email == email || x.UserName == email) &&
          x.PasswordAccount == password &&
          x.StatusAccount == "ACTIVE"
      );

      if (account == null) {
        return Json(new {
          success = false,
          message = "Email hoặc mật khẩu không đúng"
        });
      }

      // Lưu session đăng nhập
      HttpContext.Session.SetInt32("AccountId", account.AccountId);
      HttpContext.Session.SetInt32("RoleId", account.RoleId);
      HttpContext.Session.SetString("UserName", account.UserName);
      HttpContext.Session.SetString("FullName", account.NameAccount ?? "");

      return Json(new {
        success = true
      });
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
    public IActionResult EditProfile() {
      var accountId = HttpContext.Session.GetInt32("AccountId");

      var model = _context.Accounts
          .Where(a => a.AccountId == accountId)
          .Select(a => new Account {
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
    public IActionResult EditProfile(Account model) {
      try {
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
      } catch (Exception ex) {
        _logger.LogError(ex, "Lỗi cập nhật profile");
        return View(model);
      }
    }
    //thuong end code
    public IActionResult Logout() {
      return RedirectToAction("Index", "Home");
    }
    //thuong code
    public IActionResult Order(string status) {
      var accountId = HttpContext.Session.GetInt32("AccountId");
      if (accountId == null) return RedirectToAction("Login");

      // 1. Khởi tạo Query lọc theo Account trước
      var query = _context.Bills.Where(b => b.AccountId == accountId);

      // 2. Lọc trạng thái (Lọc trực tiếp trên IQueryable để SQL thực thi)
      if (!string.IsNullOrEmpty(status) && status != "Tất cả") {
        // Dùng Trim() để xử lý khoảng trắng, không dùng Normalize ở đây
        query = query.Where(b => b.StatusBill.Trim() == status.Trim());
      }

      // 3. Select dữ liệu (Giữ nguyên logic Items để lấy nguyên bảng)
      var orders = query
          .OrderByDescending(b => b.CreateAt)
          .Select(b => new OrderVM {
            BillId = b.BillId,
            TotalPrice = b.TotalPrice,
            StatusBill = b.StatusBill,
            CreateAt = b.CreateAt,
            // Quan trọng: Phải Select Items ở đây để View có dữ liệu vẽ bảng
            Items = b.BillInfos.Select(bi => new OrderItemVM {
              ProductName = bi.Variant.Product.NameProduct,
              Quantity = bi.Quantity,
              UnitPrice = bi.UnitPrice,
              ImageUrl = bi.Variant.ProductImages
                          .Where(img => img.IsMain == true)
                          .Select(img => img.UrlImage)
                          .FirstOrDefault() ?? ""
            }).ToList()
          })
          .AsNoTracking()
          .ToList();

      ViewBag.CurrentStatus = status ?? "Tất cả";
      return View(orders);
    }

    [HttpPost]
    public IActionResult CancelOrder(int id) {
      // 1. Tìm đơn hàng trong DB
      var bill = _context.Bills.Find(id);

      if (bill != null) {
        // 2. Kiểm tra điều kiện một lần nữa ở Server để bảo mật
        // Chỉ cho phép hủy khi trạng thái là "Chưa vận chuyển"
        if (bill.StatusBill?.Trim() == "Chưa vận chuyển") {
          // 3. Cập nhật trạng thái
          bill.StatusBill = "Đã hủy";

          // 4. Lưu vào SQL
          _context.SaveChanges();

          // Gợi ý: Bạn có thể thêm TempData để thông báo cho người dùng
          TempData["Message"] = "Hủy đơn hàng thành công!";
        } else {
          TempData["Error"] = "Đơn hàng đang trong quá trình vận chuyển, không thể hủy.";
        }
      }

      // Quay lại trang danh sách đơn hàng
      return RedirectToAction("Order");
    }

    //thuong end code

    //thuong code
    public IActionResult Wishlist() {
      var accountId = HttpContext.Session.GetInt32("AccountId");
      if (accountId == null) return RedirectToAction("Login", "Account");

      var model = _context.Wishlists
          .Where(w => w.AccountId == accountId)
          // Load các bảng liên quan
          .Include(w => w.ProductVariant)
              .ThenInclude(pv => pv.Product)
          .Include(w => w.ProductVariant)
              .ThenInclude(pv => pv.ProductImages)
          .Select(w => new WishlistItemVM {
            WishlistId = w.WishlistId,
            VariantId = w.ProductVariantId,
            ProductName = w.ProductVariant.Product.NameProduct, // Lấy tên từ bảng Product
            Descriptions = w.ProductVariant.Product.Descriptions, // Lấy nội dung
            Price = w.ProductVariant.Price, // Lấy giá từ bản Variant
            ImageUrl = w.ProductVariant.ProductImages
                    .Where(i => i.IsMain == true)
                    .Select(i => i.UrlImage)
                    .FirstOrDefault() ?? "default.jpg" // Ảnh mặc định nếu không có ảnh chính
          })
          .AsNoTracking()
          .ToList();

      return View(model);
    }
    [HttpPost]
    public IActionResult RemoveFromWishlist(int id) {
      // Tìm bản ghi Wishlist trong database theo ID nhận được
      var item = _context.Wishlists.Find(id);

      if (item != null) {
        // Thực hiện xóa khỏi DbContext
        _context.Wishlists.Remove(item);

        // Lưu thay đổi xuống SQL Server thực tế
        _context.SaveChanges();
      }

      // Sau khi xóa xong, quay lại trang Danh sách yêu thích
      return RedirectToAction("Wishlist");
    }
    //thuong end code

    public ActionResult EditAccount() {
      return View();
    }

    public IActionResult ChangePassword() {
      return View();
    }



  }
}
