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
    public async Task<IActionResult> List(int page = 1, int CategoryId = 0) {
      int pageSize = 15;

      var query = _context.Products.AsQueryable();

      if (CategoryId != 0) {
        query = query.Where(p => p.CategoryId == CategoryId || p.Category.ParentId == CategoryId);
      }

      var totalProducts = await query.CountAsync();

      var productCards = await query
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
                  .FirstOrDefault() ?? ""
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
      ViewBag.CategoryId = CategoryId;

      return View(model);
    }




    // GET: /Product/Detail/:id
    [HttpGet]
    public async Task<IActionResult> Detail(int id) {
      var accountId = HttpContext.Session.GetInt32("AccountId");

      var vm = await _context.Products
        .Where(p => p.ProductId == id)
        .Include(p => p.Category)
        .Include(p => p.ProductVariants).ThenInclude(v => v.Size)
        .Include(p => p.ProductVariants).ThenInclude(v => v.Color)
        .Include(p => p.ProductVariants).ThenInclude(v => v.ProductImages)
        .Include(p => p.ProductVariants)
            .ThenInclude(v => v.Comments)
                .ThenInclude(c => c.Account)
        .AsSplitQuery()
        .Select(p => new ProductDetailVM {
          Product = p,
          Variants = p.ProductVariants.ToList(),
          Images = new List<ProductImage>(),
          Comments = p.ProductVariants.SelectMany(v => v.Comments).ToList()
        })
        .FirstOrDefaultAsync();

      if (vm == null)
        return NotFound();

      var first = vm.Variants.FirstOrDefault();

      vm.SelectedVariantId = first?.VariantId ?? 0;
      vm.Price = first?.Price ?? 0;
      vm.Quanlity = first?.Quantity ?? 0;
      vm.Images = first?.ProductImages.ToList() ?? new List<ProductImage>();
      vm.AvgRating = vm.Comments.Any()
        ? vm.Comments.Average(c => c.Rating).GetValueOrDefault()
        : 0;

      vm.RelatedProducts = await _context.Products
        .Where(p => p.CategoryId == vm.Product.CategoryId
                 && p.ProductId != id
                 && p.ProductVariants.Any())
        .Include(p => p.ProductVariants)
            .ThenInclude(v => v.ProductImages)
        .OrderByDescending(p => p.ProductId)
        .Take(9)
        .ToListAsync();

      vm.IsLoggedIn = accountId != null;
      //thuong code
      // 1. Lấy danh sách ID của tất cả biến thể thuộc sản phẩm này
      var variantIds = vm.Variants.Select(v => v.VariantId).ToList();

      // 2. Đếm tổng View của sản phẩm (tổng các biến thể)
      vm.TotalViews = await _context.ProductViews
          .CountAsync(v => variantIds.Contains(v.VariantId));

      // 3. Đếm tổng Wishlist của sản phẩm (tổng các biến thể)
      // Lưu ý: Kiểm tra tên cột trong bảng Wishlist của bạn là ProductVariantId hay VariantId
      vm.TotalWishlist = await _context.Wishlists
          .CountAsync(w => variantIds.Contains(w.ProductVariantId));

      // 4. Ghi nhận 1 lượt xem mới cho biến thể mặc định (Khi user vừa nhấn vào xem)
      if (first != null) {
        var newView = new ProductView { VariantId = first.VariantId, ViewDate = DateTime.Now };
        _context.ProductViews.Add(newView);
        await _context.SaveChangesAsync();
      }
      // SỬA TẠI ĐÂY: Đổi tên biến để tránh lỗi CS0128
      var defaultVariant = vm.Variants.FirstOrDefault();

      if (defaultVariant != null) {
        // Ghi nhận lượt xem
        var newView = new ProductView {
          VariantId = defaultVariant.VariantId,
          ViewDate = DateTime.Now
        };
        _context.ProductViews.Add(newView);
        await _context.SaveChangesAsync();
      }

      // Gán dữ liệu cho VM (Sử dụng biến defaultVariant vừa đổi tên)
      vm.SelectedVariantId = defaultVariant?.VariantId ?? 0;
      vm.Price = defaultVariant?.Price ?? 0;
      vm.Quanlity = defaultVariant?.Quantity ?? 0;

      // 1. Lấy sản phẩm và đi xuyên qua các bảng liên quan
      var product = _context.Products
          .Include(p => p.ProductVariants) // Đi vào các biến thể (màu sắc, kích cỡ)
              .ThenInclude(v => v.Comments) // Từ biến thể lấy các đánh giá
                  .ThenInclude(c => c.Account) // Từ đánh giá lấy thông tin người dùng
          .FirstOrDefault(p => p.ProductId == id);

      if (product == null) return NotFound();

      // 2. Gom tất cả nhận xét từ các biến thể của sản phẩm này lại thành 1 danh sách
      var allReviews = product.ProductVariants
          .SelectMany(v => v.Comments)
          .OrderByDescending(c => c.CreateAt)
          .ToList();

      ViewBag.Reviews = allReviews;

      // 3. Kiểm tra điều kiện khách đã mua (Bill -> BillInfo -> Variant -> Product)
      ViewBag.CanReview = false;
      if (accountId != null) {
        ViewBag.CanReview = _context.Bills
            .Any(b => b.AccountId == accountId &&
                      b.StatusBill == "Completed" &&
                      b.BillInfos.Any(bi => bi.Variant.ProductId == id));
      }
      //thuong end code


      //vm.WishlistProducts = accountId != null
      //  ? await _context.Wishlists
      //      .Where(w => w.AccountId == accountId)
      //      .Include(w => w.Product)
      //          .ThenInclude(p => p.ProductVariants)
      //      .Select(w => w.Product)
      //      .ToListAsync()
      //  : new List<Product>();

      return View(vm);
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

      int pageSize = 15;
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
          .OrderBy(p => p.ProductId)
          .Skip((currentPage - 1) * pageSize)
          .Take(pageSize)
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
                  .FirstOrDefault() ??  ""
          })
          .ToListAsync();

      var totalProducts = await query.CountAsync();

      ViewBag.CurrentPage = currentPage;
      ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

      return PartialView("_FilterResultPartial", productCards);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitReview(int variantId, int rating, string content) {
      // 1. Kiểm tra đăng nhập
      var accountId = HttpContext.Session.GetInt32("AccountId");
      if (accountId == null) {
        TempData["Error"] = "Vui lòng đăng nhập để đánh giá!";
        return RedirectToAction("Login", "Account");
      }

      // 2. Kiểm tra điều kiện mua hàng (Phải có Bill thành công)
      bool canReview = _context.Bills.Any(b =>
          b.AccountId == accountId &&
          b.StatusBill == "Completed");

      if (!canReview) {
        TempData["Error"] = "Bạn chỉ có thể đánh giá sau khi nhận hàng thành công!";
        return RedirectToRoute(new { controller = "Product", action = "Detail", id = _context.ProductVariants.Find(variantId)?.ProductId });
      }

      // 3. Tạo đối tượng Comment và lưu
      var review = new Comment {
        AccountId = accountId.Value,
        VariantId = variantId, // Đảm bảo thuộc tính này map với cột Variant_Id
        Rating = rating,
        Content = content,
        CreateAt = DateTime.Now,
        StatusComment = "Show"
      };

      _context.Comments.Add(review);
      await _context.SaveChangesAsync();

      return RedirectToAction("Detail", new { id = _context.ProductVariants.Find(variantId)?.ProductId });
    }
  }

}
