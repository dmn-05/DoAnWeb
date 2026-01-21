using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Policy")]
	public class CompanyPolicyController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CompanyPolicyController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: /Admin/Policy
		[HttpGet("")]
		public IActionResult Index()
		{
			var list = _context.CompanyPolicies
				.OrderBy(x => x.DisplayOrder)
				.ToList();

			return View(list);
		}

		// GET: /Admin/Policy/Edit?code=WARRANTY
		[HttpGet("Edit")]
		public IActionResult Edit(string code)
		{
			if (string.IsNullOrEmpty(code))
				return NotFound();

			var policy = _context.CompanyPolicies
				.FirstOrDefault(x => x.Code == code);

			if (policy == null)
				return NotFound();

			return View(policy);
		}

		// POST: /Admin/Policy/Edit
		[HttpPost("Edit")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(CompanyPolicy model)
		{
			if (!ModelState.IsValid)
				return View("Edit", model);

			_context.CompanyPolicies.Update(model);
			_context.SaveChanges();

			TempData["success"] = "Cập nhật chính sách thành công";
			return RedirectToAction(nameof(Index));
		}
	}
}
