using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ProductCreateVM
  {
    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự")]
    public string NameProduct { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự")]
    public string? Descriptions { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
    public int? SupplierId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
    public int? TrademarkId { get; set; }

    public int? PromotionId { get; set; }

    [Required]
    public string StatusProduct { get; set; } = "ACTIVE";

    [Required(ErrorMessage = "Phải có ít nhất 1 biến thể")]
    public List<ProductVariantCreateVM> Variants { get; set; } = new();
  }
}
