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

		public IActionResult Index(string keyword, int page = 1)
		{
			int pageSize = 6;

			var blogs = _context.BlogPosts
					.Where(x => x.Status == "ACTIVE")
					.OrderByDescending(x => x.CreatedAt)
					.Take(9)
					.ToList();

			var productQuery = _context.ProductPosts
					.Where(x => x.Status == "ACTIVE");

			if (!string.IsNullOrEmpty(keyword))
			{
				productQuery = productQuery.Where(x =>
						x.Title.Contains(keyword) ||
						x.Summary.Contains(keyword));
			}

			int totalItems = productQuery.Count();

			var productPosts = productQuery
					.OrderByDescending(x => x.CreatedAt)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToList();

			var viewModel = new AboutViewModel
			{
				CompanyInfo = _context.CompanyInfos.FirstOrDefault(),
				Policies = _context.CompanyPolicies
							.Where(p => p.Status == "ACTIVE")
							.OrderBy(p => p.DisplayOrder)
							.ToList(),

				Blogs = blogs,
				ProductPosts = productPosts,
				CurrentPage = page,
				TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
				Keyword = keyword
			};

			return View(viewModel);
		}
	}
}
