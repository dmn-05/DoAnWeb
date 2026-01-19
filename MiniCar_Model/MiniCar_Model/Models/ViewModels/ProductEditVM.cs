namespace MiniCar_Model.Models.ViewModels
{
  public class ProductEditVM
  {
    public int ProductId { get; set; }
    public string NameProduct { get; set; }
    public string? Descriptions { get; set; }
    public string? StatusProduct { get; set; }

    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? TrademarkId { get; set; }
    public int? PromotionId { get; set; }

    // Variant
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }

    // Upload ảnh mới
    public List<IFormFile>? Images { get; set; }
    // Load ảnh cũ lên giao diện
    public List<string>? OldImages { get; set; }
  }
}
