using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

		public IActionResult Index(int page = 1)
		{
			var accountId = HttpContext.Session.GetInt32("AccountId");
			if (accountId == null)
				return RedirectToAction("Login", "Account");

			var cart = _context.Carts
					.FirstOrDefault(c => c.AccountId == accountId.Value);

			if (cart == null)
				return View(new CartViewModel());

			int pageSize = 10;

			var query = _context.CartItems
					.Where(ci => ci.CartId == cart.CartId);

			int totalItems = query.Count();
			int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			var items = query
					.OrderByDescending(ci => ci.CartItemId)
					.Skip((page - 1) * pageSize)   // QUAN TRỌNG
					.Take(pageSize)
					.Select(ci => new CartItemViewModel
					{
						CartItemId = ci.CartItemId,
						ProductName = ci.Variant.Product.NameProduct,
						ColorName = ci.Variant.Color.ColorName,
						ImageUrl = ci.Variant.ProductImages
									.Where(x => x.IsMain == true)
									.Select(x => x.UrlImage)
									.FirstOrDefault() ?? "",
						Price = ci.Price,
						Quantity = ci.Quantity
					})
					.ToList();

			return View(new CartViewModel
			{
				CartId = cart.CartId,
				Items = items,
				CurrentPage = page,
				TotalPages = totalPages,
				PageSize = pageSize
			});
		}

		//public IActionResult Add(int variantId = 0) {
		//	if (variantId !=0) {
		//		.....
		//	} else {
  //      int userId = HttpContext.Session.SetInt32("AcccountId");

  //      var CartItems = _context.CartItems
  //        .Where(ci => ci.Cart.AccountId == userId)
  //        .ToList();
  //    }
			


		//	return View(CartItem);
		//}

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
		[HttpGet]
		public IActionResult LoadMore(int skip)
		{
			var accountId = HttpContext.Session.GetInt32("AccountId");
			if (accountId == null)
				return Unauthorized();

			var cart = _context.Carts
				.FirstOrDefault(c => c.AccountId == accountId.Value);

			if (cart == null)
				return Json(new List<CartItemViewModel>());

			int pageSize = 10;

			var items = _context.CartItems
				.Where(ci => ci.CartId == cart.CartId)
				.OrderByDescending(ci => ci.CartItemId)
				.Skip(skip)
				.Take(pageSize)
				.Select(ci => new CartItemViewModel
				{
					CartItemId = ci.CartItemId,
					ProductName = ci.Variant.Product.NameProduct,
					ColorName = ci.Variant.Color.ColorName,
					ImageUrl = ci.Variant.ProductImages
						.Where(x => x.IsMain ==  true)
						.Select(x => x.UrlImage)
						.FirstOrDefault(),
					Price = ci.Price,
					Quantity = ci.Quantity
				})
				.ToList();

			return Json(items);
		}
 
    public IActionResult CheckLogin()
    {
      var accountId = HttpContext.Session.GetInt32("AccountId");

      return Json(new
      {
        isLogin = accountId != null
      });
    }

	}
}
