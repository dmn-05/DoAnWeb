using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;

public class CountProductInCartViewComponent : ViewComponent {
  private readonly ApplicationDbContext _context;

  public CountProductInCartViewComponent(ApplicationDbContext context) {
    _context = context;
  }

  public async Task<IViewComponentResult> InvokeAsync() {
    var accountId = HttpContext.Session.GetInt32("AccountId");

    int itemCount = 0;

    if (accountId != null) {
      itemCount = await _context.Carts
          .Where(c => c.AccountId == accountId && c.StatusCart == "Active")
          .SelectMany(c => c.CartItems)
          .SumAsync(ci => ci.Quantity);
    }

    return View(itemCount);
  }
}
