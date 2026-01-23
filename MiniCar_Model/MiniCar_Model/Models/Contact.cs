using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniCar_Model.Models;

public partial class Contact
{
  public int ContactId { get; set; }

  [Required(ErrorMessage = "Vui lòng nhập họ tên")]
  [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
  public string Name { get; set; } = null!;

  [Required(ErrorMessage = "Vui lòng nhập email")]
  [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
  [StringLength(150)]
  public string Email { get; set; } = null!;

  [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
  [RegularExpression(@"^(0|\+84)[0-9]{9,10}$",
      ErrorMessage = "Số điện thoại Việt Nam không hợp lệ")]
  public string? Phone { get; set; }

  [StringLength(200, ErrorMessage = "Tiêu đề tối đa 200 ký tự")]
  public string? Subject { get; set; }

  [Required(ErrorMessage = "Vui lòng nhập nội dung liên hệ")]
  [StringLength(1000, ErrorMessage = "Nội dung tối đa 1000 ký tự")]
  public string Message { get; set; } = null!;

  public string? StatusContact { get; set; }

  public DateTime? CreatedAt { get; set; }
}
