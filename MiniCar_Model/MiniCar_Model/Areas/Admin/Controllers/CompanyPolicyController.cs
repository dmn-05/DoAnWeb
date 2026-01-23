using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Policy")]
	public class CompanyPolicyController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public CompanyPolicyController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
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
		public async Task<IActionResult> Edit(
				CompanyPolicy model,
				IFormFile iconFile,
				bool RemoveIcon)
		{
			var policy = _context.CompanyPolicies
					.FirstOrDefault(x => x.Id == model.Id);

			if (policy == null)
				return NotFound();

			policy.Title = model.Title;
			policy.Description = model.Description;
			policy.Status = model.Status;
			policy.DisplayOrder = model.DisplayOrder;

			// REMOVE ICON
			if (RemoveIcon)
			{
				policy.Icon = null;
			}

			// UPLOAD ICON
			if (iconFile != null && iconFile.Length > 0)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(iconFile.FileName);
				var folder = Path.Combine(_env.WebRootPath, "uploads/policy");

				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				var path = Path.Combine(folder, fileName);
				using var stream = new FileStream(path, FileMode.Create);
				await iconFile.CopyToAsync(stream);

				policy.Icon = "/uploads/policy/" + fileName;
			}

			_context.SaveChanges();

			TempData["success"] = "Cập nhật chính sách thành công";
			return RedirectToAction(nameof(Index));
		}
	}
}
