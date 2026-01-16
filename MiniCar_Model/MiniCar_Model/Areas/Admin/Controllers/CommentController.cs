using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Areas.Admin.Controllers {
  [Area("Admin")]
  public class CommentController : Controller {
    public IActionResult Index() {
      return View();
    }
  }
}
