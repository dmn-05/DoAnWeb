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
    public string StatusProduct { get; set; } = "Active";

    // ===== Biến thể =====
    [Required(ErrorMessage = "Vui lòng chọn size")]
    public int SizeId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn màu sắc")]
    public int ColorId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá bán")]
    [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
    public int Quantity { get; set; }

    // ===== Hình ảnh =====
    [Required(ErrorMessage = "Vui lòng chọn ít nhất 1 hình ảnh")]
    public List<IFormFile>? Images { get; set; }
  }
}
