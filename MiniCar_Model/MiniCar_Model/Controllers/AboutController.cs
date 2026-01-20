using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models.ViewModels;
using MiniCar_Model.Models;

namespace MiniCar_Model.Controllers
{
	public class AboutController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AboutController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var viewModel = new AboutViewModel
			{
				CompanyInfo = _context.CompanyInfos.FirstOrDefault(),
				//Policies = _context.CompanyPolicies
				//											.Where(p => p.Status == "ACTIVE")
				//											.OrderBy(p => p.DisplayOrder)
				//											.ToList()
			};

			return View(viewModel);
		}
	}


}
