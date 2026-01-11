using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
