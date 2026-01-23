using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MiniCar_Model.Controllers {
  public class CheckoutController : Controller {
    private readonly ApplicationDbContext _context;

    public CheckoutController(ApplicationDbContext context) {
      _context = context;
    }

    public IActionResult Index(int CartId) {
      var products = _context.CartItems
        .Where(ci => ci.CartId == CartId)
        .Select(ci => new ProductCheckoutVM {
          VariantId = ci.VariantId,
          ProductName = ci.Variant.Product.NameProduct,
          ProductImage = ci.Variant.ProductImages
            .Where(img => img.IsMain == true)
            .Select(img => img.UrlImage)
            .FirstOrDefault() ?? "",
          Price = ci.Price,
          ColorName = ci.Variant.Color.ColorName,
          Quantity = ci.Quantity,
          PriceTotal = ci.Price * ci.Quantity
        })
        .ToList();

      decimal total = products.Sum(p => p.PriceTotal ?? 0);

      var cart = _context.Carts
        .Include(c => c.Account)
        .FirstOrDefault(c => c.CartId == CartId);

      var customer = cart?.Account;

      var model = new CheckoutVM {
        CartId = CartId,
        Products = products,
        Total = total,
        Customer = customer ?? new Account()
      };

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Payment(
    int CartId,
    string note,
    string paymentMethod
) {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try {
        var cart = await _context.Carts
            .Include(c => c.Account)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Variant)
            .FirstOrDefaultAsync(c => c.CartId == CartId);

        if (cart == null || !cart.CartItems.Any())
          return NotFound();

        // Kiểm tra tồn kho
        foreach (var item in cart.CartItems) {
          if (item.Quantity > item.Variant.Quantity) {
            return BadRequest(
                $"Sản phẩm {item.Variant.Product.NameProduct} không đủ số lượng tồn kho"
            );
          }
        }

        decimal totalPrice = cart.CartItems.Sum(i => i.Price * i.Quantity);

        var bill = new Bill {
          AccountId = cart.AccountId,
          CreateAt = DateTime.Now,
          NameCustomer = cart.Account.NameAccount ?? "",
          TotalPrice = totalPrice,
          PaymentDate = null,
          StatusBill = "Confirmed",
          PhoneNumber = cart.Account.PhoneNumber??"",
          DeliveryAddress = cart.Account.AddressAccount ?? "",
          CustomerNotes = note,
          PaymentMethod = paymentMethod,
          IsDeleted = false,
          BillInfos = cart.CartItems.Select(i => new BillInfo {
            VariantId = i.VariantId,
            Quantity = i.Quantity,
            UnitPrice = i.Price
          }).ToList()
        };

        _context.Bills.Add(bill);

        foreach (var item in cart.CartItems) {
          item.Variant.Quantity -= item.Quantity;
        }

        _context.CartItems.RemoveRange(cart.CartItems);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] = $"Đặt hàng thành công!";
        return RedirectToAction("Order", "Account");


      } catch (Exception ex) {
        await transaction.RollbackAsync();

        var inner = ex.InnerException?.Message ?? "NO INNER EXCEPTION";
        return BadRequest("Thanh toán thất bại: " + inner);
      }
    }

  }
}
