using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models.ViewModels
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
  }
}
