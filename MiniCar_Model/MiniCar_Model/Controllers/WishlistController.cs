using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Wishlist()
        {
            return View();
        }
    }
}
