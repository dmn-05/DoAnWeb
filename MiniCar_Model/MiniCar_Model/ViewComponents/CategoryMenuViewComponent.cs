using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;

public class CategoryMenuViewComponent : ViewComponent {
  private readonly ApplicationDbContext _context;

  public CategoryMenuViewComponent(ApplicationDbContext context) {
    _context = context;
  }

  public async Task<IViewComponentResult> InvokeAsync() {
    var categories = await _context.Categories
        .Where(c => c.StatusCategory == "Active")
        .OrderBy(c => c.NameCategory)
        .ToListAsync();

    return View(categories);
  }
}