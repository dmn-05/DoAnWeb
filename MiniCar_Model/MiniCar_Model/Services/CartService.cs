using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Services
{
  public class CartService
  {
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
      _context = context;
    }

    public CartHeaderVM GetCartHeader(int accountId)
    {
      var cart = _context.Carts
          .Include(c => c.CartItems)
          .FirstOrDefault(c =>
              c.AccountId == accountId &&
              c.StatusCart == "ACTIVE");

      if (cart == null)
      {
        return new CartHeaderVM();
      }

      return new CartHeaderVM
      {
        TotalQuantity = cart.CartItems.Sum(x => x.Quantity),
        TotalPrice = cart.CartItems.Sum(x => x.Quantity * x.Price)
      };
    }
  }
}
