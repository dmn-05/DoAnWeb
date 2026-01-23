using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;
[Area("Admin")]
public class ContactController : Controller {

  private readonly ApplicationDbContext _context;

  public ContactController(ApplicationDbContext context) {
    _context = context;
  }

  // Danh sách
  public async Task<IActionResult> Index(int page = 1) {
    int pageSize = 10;

    var query = _context.Contacts
        .Where(c => !c.IsDeleted);

    var totalContacts = await query.CountAsync();

    var contacts = await query
        .OrderByDescending(c => c.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = (int)Math.Ceiling((double)totalContacts / pageSize);

    return View(contacts);
  }


  // Form xử lý
  public async Task<IActionResult> Edit(int id) {
    var contact = await _context.Contacts.FindAsync(id);
    if (contact == null || contact.IsDeleted)
      return NotFound();

    return View(contact);
  }

  [HttpPost]
  public async Task<IActionResult> Update(int id, string newStatus) {
    var contact = await _context.Contacts.FindAsync(id);
    if (contact == null || contact.IsDeleted)
      return NotFound();

    // Trạng thái kết thúc → khóa
    if (IsFinalStatus(contact.StatusContact)) {
      TempData["error"] = "Liên hệ đã kết thúc, không thể chỉnh sửa";
      return RedirectToAction("Edit", new { id });
    }

    // Kiểm tra luồng
    if (!IsValidStatusTransition(contact.StatusContact, newStatus)) {
      TempData["error"] = "Chuyển trạng thái không hợp lệ";
      return RedirectToAction("Edit", new { id });
    }

    contact.StatusContact = newStatus;

    await _context.SaveChangesAsync();
    TempData["success"] = "Cập nhật trạng thái thành công";

    return RedirectToAction("Index");
  }

  private bool IsFinalStatus(string status) =>
      status == "Closed" || status == "Spam" || status == "Rejected";

  // LUỒNG CHUẨN
  private bool IsValidStatusTransition(string oldStatus, string newStatus) {
    return
      (oldStatus == "Pending" && newStatus == "Read") ||

      (oldStatus == "Read" &&
        (newStatus == "InProgress" ||
         newStatus == "Spam" ||
         newStatus == "Rejected")) ||

      (oldStatus == "InProgress" && newStatus == "Replied") ||

      (oldStatus == "Replied" && newStatus == "Closed");
  }

  public async Task<IActionResult> Delete(int id) {
    var contact = await _context.Contacts.FindAsync(id);
    if (contact == null)
      return NotFound();

    var allowDeleteStatuses = new[] { "Closed", "Spam", "Rejected" };

    if (!allowDeleteStatuses.Contains(contact.StatusContact)) {
      TempData["error"] = "Chỉ được xóa liên hệ đã đóng, spam hoặc bị từ chối";
      return RedirectToAction("Index");
    }

    contact.IsDeleted = true;
    contact.DeletedAt = DateTime.Now;

    await _context.SaveChangesAsync();
    TempData["success"] = "Đã xóa liên hệ";

    return RedirectToAction("Index");
  }
}
