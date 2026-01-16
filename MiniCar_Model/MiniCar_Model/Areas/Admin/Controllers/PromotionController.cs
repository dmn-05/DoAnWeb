using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
