using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ProductCreateVM
  {
    [Required]
    public string NameProduct { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? TrademarkId { get; set; }
    public int? PromotionId { get; set; }
    public string StatusProduct { get; set; } = "Active";

    // Variant (chỉ 1 biến thể đơn giản)
    [Required]
    public int SizeId { get; set; }
    [Required]
    public int ColorId { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    // Ảnh
    public List<IFormFile>? Images { get; set; }
  }
}
