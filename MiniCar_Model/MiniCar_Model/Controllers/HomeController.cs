using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        //------Tri Trong Treo
        //private readonly ApplicationDBContext _context;
        //------Tri Trong Treo
        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        //------Tri Trong Treo
        //Xu ly trang lien he tu khach hang
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Contact(Contact contact) 
        //{ 
        //    if (ModelState.IsValid)
        //    {
        //        _context.Contacts.Add(contact);
        //        await _context.SaveChangesAsync();

        //        ViewBag.Success = "Cảm ơn bạn đã liên hệ với chúng tôi";
        //        ModelState.Clear();
        //        return View();
        //    }
        //    return View(contact);
        //}
        //------Tri Trong Treo

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }


    }
}
