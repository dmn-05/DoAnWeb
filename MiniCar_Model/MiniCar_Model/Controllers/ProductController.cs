using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models.ViewModels;

namespace MiniCar_Model.Controllers {
  public class ProductController : Controller {

    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context) {
      _context = context;
    }

    // GET: /Product/List
    [HttpGet]
    public async Task<IActionResult> List(int page = 1) {
      int pageSize = 9;
      var totalProducts = await _context.Products.CountAsync();

      var products = await _context.Products
        .OrderBy(p => p.ProductId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      var sizes = await _context.Sizes.ToListAsync();

      var trademarks = await _context.Trademarks.ToListAsync();

      var model = new ProductFilterVM { 
        Products = products,
        Sizes = sizes,
        Trademarks = trademarks 
      };

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
      return View(model);
    }

    // GET: /Product/Detail/:id
    [HttpGet]
    public IActionResult Detail() {
      return View();
    }

    public IActionResult Search(ProductSearchVM vm) {

      var query = _context.ProductVariants
          .Include(v => v.Product)
              .ThenInclude(p => p.Category)
          .Include(v => v.Size)
          .Include(v => v.Color)
          .AsQueryable();

      // 🔎 Tìm theo từ khoá (Tên + mô tả Product)
      if (!string.IsNullOrEmpty(vm.Keyword)) {
        query = query.Where(v =>
            v.Product.NameProduct.Contains(vm.Keyword) ||
            v.Product.Descriptions.Contains(vm.Keyword)
        );
      }

      // 📂 Theo danh mục
      if (vm.CategoryId.HasValue) {
        query = query.Where(v =>
            v.Product.CategoryId == vm.CategoryId
        );
      }

      // 💰 Giá từ
      if (vm.MinPrice.HasValue) {
        query = query.Where(v => v.Price >= vm.MinPrice);
      }

      // 💰 Giá đến
      if (vm.MaxPrice.HasValue) {
        query = query.Where(v => v.Price <= vm.MaxPrice);
      }

      // Chỉ lấy variant đang active
      query = query.Where(v =>
          v.StatusVariant == "ACTIVE" &&
          v.Product.StatusProduct == "ACTIVE"
      );

      vm.Results = query.ToList();

      ViewBag.Categories = _context.Categories
          .Where(c => c.StatusCategory == "ACTIVE")
          .ToList();

      return View(vm);
    }

  }
}
