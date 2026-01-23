using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class ContactCreateVM
  {
    [Required(ErrorMessage = "Vui lòng nhập tên")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    public string? Subject { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung")]
    public string Message { get; set; } = null!;
  }
}
