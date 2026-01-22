using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCar_Model.Models;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

      var productCards = await _context.Products
          .OrderBy(p => p.ProductId)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(p => new ProductCardVM {
            ProductId = p.ProductId,
            NameProduct = p.NameProduct,

            VariantId = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.VariantId)
                  .FirstOrDefault(),

            Price = p.ProductVariants
                  .OrderBy(v => v.VariantId)
                  .Select(v => v.Price)
                  .FirstOrDefault(),

            ImageUrl = p.ProductVariants
                  .SelectMany(v => v.ProductImages)
                  .OrderByDescending(i => i.IsMain)
                  .Select(i => i.UrlImage)
                  .FirstOrDefault()
          })
          .ToListAsync();

      var model = new ProductFilterVM {
        Products = productCards,
        Sizes = await _context.Sizes.ToListAsync(),
        Colors = await _context.Colors.ToListAsync(),
        Trademarks = await _context.Trademarks.ToListAsync()
      };

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

      return View(model);
    }



    // GET: /Product/Detail/:id
    [HttpGet]
    public IActionResult Detail(int variantId) {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Filter(
    string keyword,
    int? trademarkId,
    int? sizeId,
    int? colorId,
    decimal? minPrice,
    decimal? maxPrice,
    int? page
) {

      int pageSize = 9;
      int currentPage = page ?? 1;
      var query = _context.Products
          .Include(p => p.ProductVariants)
              .ThenInclude(v => v.ProductImages)
          .AsQueryable();

      // 1. Lọc theo các thuộc tính của Product
      if (!string.IsNullOrWhiteSpace(keyword)) {
        query = query.Where(p => p.NameProduct.Contains(keyword));
      }

      if (trademarkId.HasValue) {
        query = query.Where(p => p.TrademarkId == trademarkId);
      }

      // 2. Lọc theo các thuộc tính của Variant (nếu có chọn lọc)
      if (sizeId.HasValue || colorId.HasValue || minPrice.HasValue || maxPrice.HasValue) {
        query = query.Where(p => p.ProductVariants.Any(v =>
            (!sizeId.HasValue || v.SizeId == sizeId) &&
            (!colorId.HasValue || v.ColorId == colorId) &&
            (!minPrice.HasValue || v.Price >= minPrice) &&
            (!maxPrice.HasValue || v.Price <= maxPrice)
        ));
      }

      // 3. Projection dữ liệu ra ViewModel
      var productCards = await query
          .Select(p => new ProductCardVM {
            ProductId = p.ProductId,
            NameProduct = p.NameProduct,

            // Lấy Variant thỏa mãn bộ lọc, nếu không lọc thì lấy cái đầu tiên
            // Sử dụng Let hoặc Sub-query an toàn hơn
            Price = p.ProductVariants
                  .Where(v => (!sizeId.HasValue || v.SizeId == sizeId) &&
                              (!colorId.HasValue || v.ColorId == colorId) &&
                              (!minPrice.HasValue || v.Price >= minPrice) &&
                              (!maxPrice.HasValue || v.Price <= maxPrice))
                  .OrderBy(v => v.Price)
                  .Select(v => v.Price)
                  .FirstOrDefault(),

            VariantId = p.ProductVariants
                  .Where(v => (!sizeId.HasValue || v.SizeId == sizeId) &&
                              (!colorId.HasValue || v.ColorId == colorId) &&
                              (!minPrice.HasValue || v.Price >= minPrice) &&
                              (!maxPrice.HasValue || v.Price <= maxPrice))
                  .OrderBy(v => v.Price)
                  .Select(v => v.VariantId)
                  .FirstOrDefault(),

            ImageUrl = p.ProductVariants
                  .Where(v => (!sizeId.HasValue || v.SizeId == sizeId) &&
                              (!colorId.HasValue || v.ColorId == colorId) &&
                              (!minPrice.HasValue || v.Price >= minPrice) &&
                              (!maxPrice.HasValue || v.Price <= maxPrice))
                  .SelectMany(v => v.ProductImages)
                  .OrderByDescending(i => i.IsMain)
                  .Select(i => i.UrlImage)
                  .FirstOrDefault()
          })
          .ToListAsync();

      var totalProducts = await query.CountAsync();

      ViewBag.CurrentPage = currentPage;
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

      return PartialView("_FilterResultPartial", productCards);
    }
  }

}
