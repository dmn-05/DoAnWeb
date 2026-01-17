using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Services;
using MiniCar_Model.Models.ViewModels;
using System.Security.Claims;

namespace MiniCar_Model.ViewComponents
{
  public class CartHeaderViewComponent : ViewComponent
  {
    private readonly CartService _cartService;

    public CartHeaderViewComponent(CartService cartService)
    {
      _cartService = cartService;
    }

    public IViewComponentResult Invoke()
    {
      if (!HttpContext.User.Identity!.IsAuthenticated)
      {
        return View(new CartHeaderVM());
      }

      var accountIdClaim =
          HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

      if (accountIdClaim == null)
      {
        return View(new CartHeaderVM());
      }

      int accountId = int.Parse(accountIdClaim.Value);

      var vm = _cartService.GetCartHeader(accountId);
      return View(vm);
    }
  }
}
