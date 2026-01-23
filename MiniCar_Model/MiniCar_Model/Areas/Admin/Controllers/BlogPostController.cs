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

		public IActionResult Index()
		{
			var blogs = _context.BlogPosts
				.OrderByDescending(x => x.CreatedAt)
				.ToList();

			return View(blogs); 
		}


		public IActionResult Edit(int id)
		{
			var blog = _context.BlogPosts.FirstOrDefault(x => x.Id == id);

			if (blog == null)
				return NotFound();

			return View(blog); 
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(
		BlogPost model,
		IFormFile thumbnailFile,
		bool RemoveThumbnail)
		{
			var blog = _context.BlogPosts.Find(model.Id);
			if (blog == null) return NotFound();

			blog.Title = model.Title;
			blog.Summary = model.Summary;
			blog.Content = model.Content;
			blog.Status = model.Status;
			blog.UpdatedAt = DateTime.Now;

			// ====== XỬ LÝ XÓA ẢNH ======
			if (RemoveThumbnail && !string.IsNullOrEmpty(blog.Thumbnail))
			{
				var oldPath = Path.Combine(
						Directory.GetCurrentDirectory(),
						"wwwroot",
						blog.Thumbnail.TrimStart('/'));

				if (System.IO.File.Exists(oldPath))
					System.IO.File.Delete(oldPath);

				blog.Thumbnail = null;
			}

			// ====== XỬ LÝ UPLOAD ẢNH ======
			if (thumbnailFile != null && thumbnailFile.Length > 0)
			{
				var folder = Path.Combine("wwwroot/uploads/blog");
				Directory.CreateDirectory(folder);

				var fileName = Guid.NewGuid() + Path.GetExtension(thumbnailFile.FileName);
				var filePath = Path.Combine(folder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					thumbnailFile.CopyTo(stream);
				}

				blog.Thumbnail = "/uploads/blog/" + fileName;
			}

			// ====== SLUG ======
			blog.Slug = model.Title
					.ToLower()
					.Trim()
					.Replace(" ", "-");

			_context.SaveChanges();
			return RedirectToAction("Index");
		}


		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(BlogPost model, IFormFile thumbnailFile)
		{
			// UPLOAD TRƯỚC
			if (thumbnailFile != null && thumbnailFile.Length > 0)
			{
				var folder = Path.Combine("wwwroot/uploads/blog");
				Directory.CreateDirectory(folder);

				var fileName = Guid.NewGuid() + Path.GetExtension(thumbnailFile.FileName);
				var filePath = Path.Combine(folder, fileName);

				using var stream = new FileStream(filePath, FileMode.Create);
				thumbnailFile.CopyTo(stream);

				model.Thumbnail = "/uploads/blog/" + fileName;
			}

			// SAU ĐÓ MỚI CHECK MODEL
			if (!ModelState.IsValid)
				return View(model);

			model.Slug = model.Title.ToLower().Trim().Replace(" ", "-");
			model.CreatedAt = DateTime.Now;
			model.UpdatedAt = DateTime.Now;

			_context.BlogPosts.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}


