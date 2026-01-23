using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Filters;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;

namespace MiniCar_Model.Areas.Admin.Controllers {
  [Area("Admin")]
  [AdminAuthorize]
  public class CommentController : Controller {

    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CommentController(ApplicationDbContext context, IWebHostEnvironment env)
    {
      _context = context;
      _env = env;
    }

    public async Task<IActionResult> Index(string search, int page = 1) {
      int pageSize = 10;

      var query = _context.Comments
          .Include(c => c.Account)
          .Include(c => c.Variant)
          .AsQueryable();

      // 🔍 Search theo nội dung comment hoặc tên tài khoản
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(c =>
            c.Content!.Contains(search) ||
            c.Account.UserName.Contains(search)
        );
      }

      int totalItems = await query.CountAsync();

      var comments = await query
          .OrderByDescending(c => c.CreateAt)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(c => new CommentIndexVM
          {
            CommentId = c.CommentId,
            AccountName = c.Account.UserName,
            VariantId = c.VariantId,
            Rating = c.Rating,
            Content = c.Content,
            CreateAt = c.CreateAt,
            StatusComment = c.StatusComment
          })
          .ToListAsync();

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
      ViewBag.Search = search;

      return View(comments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var comment = await _context.Comments.FindAsync(id);
      if (comment == null)
        return NotFound();

      comment.StatusComment = "INACTIVE";
      comment.UpdateAt = DateTime.Now;

      await _context.SaveChangesAsync();

      TempData["success"] = "Xóa bình luận thành công";
      return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
      var comment = await _context.Comments.FindAsync(id);
      if (comment == null) return NotFound();

      comment.StatusComment = "ACTIVE";
      comment.UpdateAt = DateTime.Now;

      await _context.SaveChangesAsync();

      TempData["success"] = "Khôi phục bình luận thành công";
      return RedirectToAction(nameof(Index));
    }

  }
}
