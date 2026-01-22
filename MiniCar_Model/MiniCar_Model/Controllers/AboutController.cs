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

    public IActionResult Index(string keyword, int blogPage = 1, int productPage = 1)
    {
      int blogPageSize = 3;
      int productPageSize = 6;

      // ===== BLOG =====
      var blogQuery = _context.BlogPosts
          .Where(x => x.Status == "ACTIVE")
          .OrderByDescending(x => x.CreatedAt);

      int totalBlog = blogQuery.Count();

      var blogs = blogQuery
          .Skip((blogPage - 1) * blogPageSize)
          .Take(blogPageSize)
          .ToList();

      // ===== PRODUCT =====
      var productQuery = _context.ProductPosts
          .Where(x => x.Status == "ACTIVE");

      if (!string.IsNullOrEmpty(keyword))
      {
        productQuery = productQuery.Where(x =>
            x.Title.Contains(keyword) ||
            x.Summary.Contains(keyword));
      }

      int totalProduct = productQuery.Count();

      var productPosts = productQuery
          .OrderByDescending(x => x.CreatedAt)
          .Skip((productPage - 1) * productPageSize)
          .Take(productPageSize)
          .ToList();

      var viewModel = new AboutViewModel
      {
        CompanyInfo = _context.CompanyInfos.FirstOrDefault(),

        Policies = _context.CompanyPolicies
              .Where(p => p.Status == "ACTIVE")
              .OrderBy(p => p.DisplayOrder)
              .ToList(),

        Blogs = blogs,
        BlogCurrentPage = blogPage,
        BlogTotalPages = (int)Math.Ceiling((double)totalBlog / blogPageSize),

        ProductPosts = productPosts,
        ProductCurrentPage = productPage,
        ProductTotalPages = (int)Math.Ceiling((double)totalProduct / productPageSize),

        Keyword = keyword
      };

      return View(viewModel);
    }
    [HttpGet]
    public IActionResult BlogPaging(int blogPage = 1)
    {
      int pageSize = 3;

      var query = _context.BlogPosts
          .Where(x => x.Status == "ACTIVE")
          .OrderByDescending(x => x.CreatedAt);

      int totalItems = query.Count();

      var blogs = query
          .Skip((blogPage - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      var vm = new AboutViewModel
      {
        Blogs = blogs,
        BlogCurrentPage = blogPage,
        BlogTotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
      };

      return PartialView("_BlogList", vm);
    }
		[HttpGet]
		public IActionResult ProductPaging(int productPage = 1, string keyword = null)
		{
			int pageSize = 6;

			var query = _context.ProductPosts
				.Where(x => x.Status == "ACTIVE");

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(x =>
					x.Title.Contains(keyword) ||
					x.Summary.Contains(keyword));
			}

			int totalItems = query.Count();

			var productPosts = query
				.OrderByDescending(x => x.CreatedAt)
				.Skip((productPage - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var vm = new AboutViewModel
			{
				ProductPosts = productPosts,
				ProductCurrentPage = productPage,
				ProductTotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
				Keyword = keyword
			};

			return PartialView("_ProductList", vm);
		}
	}
 }
