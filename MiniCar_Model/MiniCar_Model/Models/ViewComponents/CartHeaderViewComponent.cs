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
		int totalItems = 0;

		var accountId = HttpContext.Session.GetInt32("AccountId");

		if (accountId != null)
		{
			totalItems = _context.CartItems
					.Where(ci => ci.Cart.AccountId == accountId)
					.Count();
		}

		var vm = new CartHeaderVM
		{
			TotalQuantity = totalItems
		};

		return View(vm);
	}


}
