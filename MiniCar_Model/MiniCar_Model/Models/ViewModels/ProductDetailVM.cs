namespace MiniCar_Model.Models.ViewModels
{
  public class ProductDetailVM
  {
    public Product Product { get; set; }

    // Danh sách biến thể
    public List<ProductVariant> Variants { get; set; }

    // Variant đang chọn
    public int? SelectedVariantId { get; set; }
    public decimal? SelectedVariantPrice { get; set; }
    public int? SelectedVariantStock { get; set; }

    // Ảnh theo variant
    public List<ProductImage> VariantImages { get; set; }

    public decimal Price { get; set; }
    public int Quanlity { get; set; }

    //// Giá hiển thị ban đầu
    //public decimal MinPrice { get; set; }
    //public decimal MaxPrice { get; set; }

    // Đánh giá
    public List<Comment> Comments { get; set; }
    public double AvgRating { get; set; }

    // Sản phẩm liên quan
    public List<Product> RelatedProducts { get; set; }

    // Wishlist
    public bool IsFavorite { get; set; }

    // Auth
    public bool IsLoggedIn { get; set; }

    public int TotalViews { get; set; }

    public int TotalWishlist {  get; set; }

  }
}
