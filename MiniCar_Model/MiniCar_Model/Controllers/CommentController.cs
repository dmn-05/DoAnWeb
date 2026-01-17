using Microsoft.AspNetCore.Mvc;

namespace MiniCar_Model.Controllers
{
	public class CommentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
