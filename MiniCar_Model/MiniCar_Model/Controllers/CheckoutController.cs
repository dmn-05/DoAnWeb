using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
