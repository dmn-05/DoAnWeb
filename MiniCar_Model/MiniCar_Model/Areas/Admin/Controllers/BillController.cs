using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Areas.Admin.Controllers {
  [Area("Admin")]
  public class BillController : Controller {

    private readonly ApplicationDbContext _context;

    public BillController(ApplicationDbContext context) {
      _context = context;
    }

    // Danh sách hóa đơn
    public async Task<IActionResult> Index(int page = 1) {

  int pageSize = 10;

  var query = _context.Bills
      .Include(b => b.Account)
      .Where(b => !b.IsDeleted);

  var totalBills = await query.CountAsync();

  var bills = await query
      .OrderBy(b => b.BillId)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

  ViewBag.CurrentPage = page;
  ViewBag.TotalPages = (int)Math.Ceiling((double)totalBills / pageSize);

  return View(bills);
}

    // GET: Hiển thị form cập nhật
    public async Task<IActionResult> Edit(int id) {
      var bill = await _context.Bills.FindAsync(id);
      if (bill == null) return NotFound();

      return View(bill);
    }

    // POST: Cập nhật hóa đơn
    [HttpPost]
    public async Task<IActionResult> Update(int id, string newStatus) {
      var bill = await _context.Bills.FindAsync(id);
      if (bill == null) return NotFound();

      //  Không cho sửa khi đã hoàn thành
      if (bill.StatusBill == "Completed") {
        TempData["error"] = "Hóa đơn đã hoàn thành, không thể chỉnh sửa";
        return RedirectToAction("Edit", new { id });
      }

      // Kiểm tra luồng trạng thái hợp lệ
      if (!IsValidStatusTransition(bill.StatusBill, newStatus)) {
        TempData["error"] = "Chuyển trạng thái không hợp lệ";
        return RedirectToAction("Edit", new { id });
      }

      // Cập nhật trạng thái
      bill.StatusBill = newStatus;

      // Set ngày thanh toán
      if (newStatus == "Paid") {
        bill.PaymentDate = DateTime.Now;
      }

      // Hoàn tiền
      if (newStatus == "Refunded") {
        bill.PaymentDate = DateTime.Now;
      }

      await _context.SaveChangesAsync();
      TempData["success"] = "Cập nhật hóa đơn thành công";

      return RedirectToAction("Index");
    }

    // Kiểm tra luồng
    private bool IsValidStatusTransition(string oldStatus, string newStatus) {
      return (oldStatus == "Pending" && (newStatus == "Confirmed" || newStatus == "Cancelled"))
          || (oldStatus == "Confirmed" && newStatus == "Paid")
          || (oldStatus == "Paid" && (newStatus == "Completed" || newStatus == "Refunded"));
    }

    public async Task<IActionResult> Delete(int id) {
      var bill = await _context.Bills.FindAsync(id);
      if (bill == null) return NotFound();

      // Không cho xóa hóa đơn đã hoàn thành
      if (bill.StatusBill == "Completed") {
        TempData["error"] = "Không thể xóa hóa đơn đã hoàn thành";
        return RedirectToAction("Index");
      }

      bill.IsDeleted = true;
      bill.DeletedAt = DateTime.Now;

      await _context.SaveChangesAsync();

      TempData["success"] = "Xóa hóa đơn thành công";
      return RedirectToAction("Index");
    }
  }
}
