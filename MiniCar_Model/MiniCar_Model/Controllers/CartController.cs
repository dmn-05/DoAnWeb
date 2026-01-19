using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;
using System.Linq;

namespace MiniCar_Model.Controllers
{
  public class CartController : Controller
  {
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      // 1. Kiểm tra đăng nhập bằng Session
      var accountId = HttpContext.Session.GetInt32("AccountId");
      if (accountId == null)
      {
        return RedirectToAction("Login", "Account");
      }

      // 2. Lấy Cart theo Account
      var cart = _context.Carts
          .AsNoTracking()
          .FirstOrDefault(c => c.AccountId == accountId.Value);

      if (cart == null)
      {
        return View(new CartViewModel());
      }

      // 3. Lấy CartItem
      var items = _context.CartItems
          .AsNoTracking()
          .Where(ci => ci.CartId == cart.CartId)
          .Include(ci => ci.Variant)
              .ThenInclude(v => v.Product)
          .Include(ci => ci.Variant)
              .ThenInclude(v => v.Color)
          .Include(ci => ci.Variant)
              .ThenInclude(v => v.ProductImages)
          .Select(ci => new CartItemViewModel
          {
            CartItemId = ci.CartItemId,
            VariantId = ci.VariantId,
            ProductName = ci.Variant.Product.NameProduct,
            ColorName = ci.Variant.Color.ColorName,
            ImageUrl = ci.Variant.ProductImages
                  .Where(img => img.IsMain == true)
                  .Select(img => img.UrlImage)
                  .FirstOrDefault(),
            Price = ci.Price,
            Quantity = ci.Quantity
          })
          .ToList();

      return View(new CartViewModel
      {
        Items = items
      });
    }

		[HttpPost]
		public IActionResult RemoveItem(int cartItemId)
		{
			var item = _context.CartItems
				.Include(ci => ci.Cart)
				.FirstOrDefault(x => x.CartItemId == cartItemId);

			if (item == null)
				return NotFound();

			_context.CartItems.Remove(item);
			_context.SaveChanges();

			var accountId = HttpContext.Session.GetInt32("AccountId");
			int totalItems = 0;

			if (accountId != null)
			{
				totalItems = _context.CartItems
					.Count(ci => ci.Cart.AccountId == accountId);
			}

			return Json(new { totalItems });
		}


		[HttpPost]
		public IActionResult RemoveSelected([FromBody] List<int> ids)
		{
			if (ids == null || ids.Count == 0)
				return BadRequest();

			var items = _context.CartItems
					.Include(ci => ci.Cart)
					.Where(x => ids.Contains(x.CartItemId))
					.ToList();

			if (!items.Any())
				return BadRequest();

			var cart = items.First().Cart;

			_context.CartItems.RemoveRange(items);
			_context.SaveChanges();

			bool cartIsEmpty = !_context.CartItems
					.Any(ci => ci.CartId == cart.CartId);

			if (cartIsEmpty)
			{
				_context.Carts.Remove(cart);
				_context.SaveChanges();
			}

			var accountId = HttpContext.Session.GetInt32("AccountId");

			int totalItems = 0;
			if (accountId != null)
			{
				totalItems = _context.CartItems
						.Count(ci => ci.Cart.AccountId == accountId);
			}

			return Json(new { totalItems });
		}
	}
}
