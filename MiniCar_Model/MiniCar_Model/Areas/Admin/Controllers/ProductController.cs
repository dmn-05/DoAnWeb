using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }    
        public IActionResult Create()
        {
            return View();
        }
    }


}
