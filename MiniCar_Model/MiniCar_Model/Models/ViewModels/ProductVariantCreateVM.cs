using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ProductVariantCreateVM
  {
    [Required(ErrorMessage = "Vui lòng chọn size")]
    public int SizeId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn màu sắc")]
    public int ColorId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá")]
    [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
    public int Quantity { get; set; }

    [Required]
    public string StatusVariant { get; set; } = "ACTIVE";

    [Required(ErrorMessage = "Phải chọn ít nhất 1 hình")]
    public List<IFormFile>? Images { get; set; }
  }
}
