using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ProductVariantEditVM
  {
    public int VariantId { get; set; }
    public int ProductId { get; set; }

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

    // Ảnh mới (không bắt buộc)
    public List<IFormFile>? Images { get; set; }

    // Ảnh hiện tại để preview
    public List<string>? ExistingImages { get; set; }
  }
}
