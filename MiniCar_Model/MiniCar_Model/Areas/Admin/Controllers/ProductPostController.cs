using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductPostController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ProductPostController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var posts = _context.ProductPosts
													.OrderByDescending(x => x.CreatedAt)
													.ToList();

			return View(posts);
		}
		public IActionResult Edit(int id)
		{
			var post = _context.ProductPosts.FirstOrDefault(x => x.Id == id);

			if (post == null)
				return NotFound();

			return View(post);
		}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
    ProductPost model,
    IFormFile thumbnailFile,
    bool RemoveThumbnail
)
    {
      var post = _context.ProductPosts.Find(model.Id);
      if (post == null)
        return NotFound();

      // ===== UPDATE FIELD =====
      post.Title = model.Title;
      post.Summary = model.Summary;
      post.Content = model.Content;
      post.Status = model.Status;

      // ===== REMOVE OLD THUMB =====
      if (RemoveThumbnail && !string.IsNullOrEmpty(post.Thumbnail))
      {
        var oldPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            post.Thumbnail.TrimStart('/')
        );

        if (System.IO.File.Exists(oldPath))
          System.IO.File.Delete(oldPath);

        post.Thumbnail = null;
      }

      // ===== UPLOAD NEW THUMB =====
      if (thumbnailFile != null && thumbnailFile.Length > 0)
      {
        var folder = Path.Combine("wwwroot/uploads/products");
        Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + Path.GetExtension(thumbnailFile.FileName);
        var filePath = Path.Combine(folder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          thumbnailFile.CopyTo(stream);
        }

        post.Thumbnail = "/uploads/products/" + fileName;
      }

      // ===== SLUG =====
      post.Slug = model.Title
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
    public IActionResult Create(ProductPost model, IFormFile thumbnailFile)
    {
      // ===== UPLOAD ẢNH =====
      if (thumbnailFile != null && thumbnailFile.Length > 0)
      {
        var folder = Path.Combine("wwwroot/uploads/products");
        Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + Path.GetExtension(thumbnailFile.FileName);
        var filePath = Path.Combine(folder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        thumbnailFile.CopyTo(stream);

        model.Thumbnail = "/uploads/products/" + fileName;
      }

      if (!ModelState.IsValid)
        return View(model);

      model.Slug = model.Title.ToLower().Trim().Replace(" ", "-");
      model.CreatedAt = DateTime.Now;

      _context.ProductPosts.Add(model);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }
  }
}
