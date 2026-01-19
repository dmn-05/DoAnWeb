using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models.ViewModels;
using MiniCar_Model.Models;

public class CartHeaderViewComponent : ViewComponent
{
	private readonly ApplicationDbContext _context;

	public CartHeaderViewComponent(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		int totalQuantity = 0;

		var accountId = HttpContext.Session.GetInt32("AccountId");

		if (accountId != null)
		{
			totalQuantity = _context.CartItems
					.Where(ci => ci.Cart.AccountId == accountId)
					.Sum(ci => ci.Quantity);
		}

		var vm = new CartHeaderVM
		{
			TotalQuantity = totalQuantity
		};

		return View(vm);
	}

}
