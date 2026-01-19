namespace MiniCar_Model.Models.ViewModels
{
  public class ProductDetailVM
  {
    public Product Product { get; set; }
    public List<ProductVariant> Variants { get; set; }
    public List<ProductImage> Images { get; set; }
    public int SelectedVariantId { get; set; }
    public decimal  Price { get; set; }
    public int Quanlity { get; set; }
    public List<Comment> Comments { get; set; }
    public double AvgRating { get; set; }

    public List<Product> RelatedProducts { get; set; }

    public List<Product> WishlistProducts { get; set; } = new();

  }
}
