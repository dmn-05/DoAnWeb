using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class CategoryController : Controller
  {
    //thuong code 
    private readonly ApplicationDbContext _context;
    public CategoryController(ApplicationDbContext context)
    {
      _context = context;
    }
    //thuong end code
    //thuong code
    // --- 1. DANH SÁCH ---

    public IActionResult Index(string searchString, int page = 1)
    {
      int pageSize = 10;
      int offset = (page - 1) * pageSize;

      var query = _context.Categories.AsQueryable();

      // Xử lý tìm kiếm (giữ nguyên logic cũ)
      if (!string.IsNullOrEmpty(searchString))
      {
        query = query.Where(c => c.NameCategory.Contains(searchString)
                              || c.CategoryId.ToString() == searchString);
        ViewBag.SearchString = searchString;
      }

      int totalItems = query.Count();
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
      ViewBag.CurrentPage = page;

      // SẮP XẾP TỐI ƯU: Ưu tiên UpdatedAt, nếu NULL thì dùng CreatedAt
      var categories = query
          .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
          .Skip(offset)
          .Take(pageSize)
          .ToList();

      return View(categories);
    }
    [HttpGet] // Thêm thuộc tính này nếu chưa có
    public IActionResult Create()
    {
      // Nạp danh sách danh mục cha để hiển thị Dropdown
      ViewBag.ParentCategories = _context.Categories
          .Where(c => c.StatusCategory == "ACTIVE")
          .ToList();
      return View();
    }

    // 2. Hàm này dùng để xử lý dữ liệu khi người dùng nhấn nút "Lưu"
    [HttpPost] // Phải có thuộc tính này để phân biệt với hàm Get ở trên
    [ValidateAntiForgeryToken] // Nên thêm để bảo mật
    public IActionResult Create(Category category)
    {
      if (ModelState.IsValid)
      {
        category.CreatedAt = DateTime.Now;
        category.StatusCategory = "ACTIVE"; //

        _context.Categories.Add(category);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
      }
      return View(category);
    }

    // --- 3. CHỈNH SỬA ---
    [HttpGet]
    public IActionResult Edit(int id)
    {
      var category = _context.Categories.Find(id);
      if (category == null) return NotFound();

      ViewBag.ParentCategories = _context.Categories
          .Where(c => c.StatusCategory == "ACTIVE" && c.CategoryId != id)
          .ToList();
      return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
      var existing = _context.Categories.Find(category.CategoryId);
      if (existing == null) return NotFound();

      existing.NameCategory = category.NameCategory;
      existing.ParentId = category.ParentId;
      existing.StatusCategory = category.StatusCategory;
      existing.UpdatedAt = DateTime.Now;

      _context.Update(existing);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }

    // --- 4. XÓA (Đổi trạng thái ACTIVE -> INACTIVE) ---
    [HttpPost]
    public IActionResult ChangeStatus(int id)
    {
      var category = _context.Categories.Find(id);
      if (category != null)
      {
        // Đảo trạng thái giữa ACTIVE và INACTIVE
        category.StatusCategory = (category.StatusCategory == "ACTIVE") ? "INACTIVE" : "ACTIVE";
        category.UpdatedAt = DateTime.Now;

        _context.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    //thuong end code
  }
}
