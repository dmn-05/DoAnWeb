using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
    public class DonHangController : Controller
    {
        public IActionResult DonHang()
        {
            return View();
        }
    }
}
