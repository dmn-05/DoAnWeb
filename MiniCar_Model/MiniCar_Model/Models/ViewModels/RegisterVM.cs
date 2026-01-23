using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class RegisterVM
  {
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string NameAccount { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [StringLength(10, ErrorMessage = "Số điện thoại tối đa 10 số")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    public string AddressAccount { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [DataType(DataType.Password)]
    public string PasswordAccount { get; set; }

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
    [DataType(DataType.Password)]
    [Compare("PasswordAccount", ErrorMessage = "Mật khẩu không khớp")]
    public string ConfirmPassword { get; set; }
  }
}
