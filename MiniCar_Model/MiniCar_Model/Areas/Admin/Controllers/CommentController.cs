using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Filters;

namespace MiniCar_Model.Areas.Admin.Controllers {
  [Area("Admin")]
  [AdminAuthorize]
  public class CommentController : Controller {
    public IActionResult Index() {
      return View();
    }
  }
}
