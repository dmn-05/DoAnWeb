using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ProductEditVM
  {
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    [StringLength(200)]
    public string NameProduct { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Descriptions { get; set; }

    [Required]
    public int? CategoryId { get; set; }

    [Required]
    public int? SupplierId { get; set; }

    [Required]
    public int? TrademarkId { get; set; }

    public int? PromotionId { get; set; }

    [Required]
    public string StatusProduct { get; set; } = "ACTIVE";
  }
}
