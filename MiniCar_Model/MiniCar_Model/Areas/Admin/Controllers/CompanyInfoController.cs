using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CompanyInfoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CompanyInfoController(ApplicationDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Edit()
		{
			var model = _context.CompanyInfos.FirstOrDefault();

			if (model == null)
			{
				model = new CompanyInfo();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CompanyInfo model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var entity = _context.CompanyInfos.FirstOrDefault();

			if (entity == null)
			{
				entity = new CompanyInfo();
				_context.CompanyInfos.Add(entity);
			}

			entity.CompanyName = model.CompanyName;
			entity.BusinessField = model.BusinessField;
			entity.Address = model.Address;
			entity.Email = model.Email;
			entity.Hotline = model.Hotline;
			entity.Description = model.Description;
			entity.UpdatedAt = DateTime.Now;

			_context.SaveChanges();

			TempData["success"] = "Cập nhật thông tin công ty thành công";
			return RedirectToAction(nameof(Edit));
		}
	}
}
