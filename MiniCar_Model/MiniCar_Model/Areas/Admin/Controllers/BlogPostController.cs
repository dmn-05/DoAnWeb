using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using System;
using System.Linq;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BlogPostController : Controller
	{
		private readonly ApplicationDbContext _context;

		public BlogPostController(ApplicationDbContext context)
		{
			_context = context;
		}

		// LIST
		public IActionResult Index()
		{
			var blogs = _context.BlogPosts
					.OrderByDescending(x => x.CreatedAt)
					.ToList();

			return View(blogs);
		}

		// CREATE - GET
		public IActionResult Create()
		{
			return View();
		}

		// CREATE - POST
		[HttpPost]
		public IActionResult Create(BlogPost model)
		{
			if (ModelState.IsValid)
			{
				model.Slug = model.Title.ToLower().Replace(" ", "-");
				model.Status = "ACTIVE";
				model.CreatedAt = DateTime.Now;

				_context.BlogPosts.Add(model);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(model);
		}

		// EDIT - GET
		public IActionResult Edit(int id)
		{
			var blog = _context.BlogPosts.Find(id);
			if (blog == null) return NotFound();

			return View(blog);
		}

		// EDIT - POST
		[HttpPost]
		public IActionResult Edit(BlogPost model)
		{
			if (ModelState.IsValid)
			{
				var blog = _context.BlogPosts.Find(model.Id);
				if (blog == null) return NotFound();

				blog.Title = model.Title;
				blog.Summary = model.Summary;
				blog.Content = model.Content;
				blog.Thumbnail = model.Thumbnail;
				blog.Slug = model.Title.ToLower().Replace(" ", "-");

				_context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(model);
		}

		// SOFT DELETE (update Status)
		[HttpPost]
		public IActionResult Delete(int id)
		{
			var blog = _context.BlogPosts.Find(id);
			if (blog == null) return NotFound();

			blog.Status = "INACTIVE";
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
