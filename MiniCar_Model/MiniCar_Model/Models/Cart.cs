using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Cart")]
[Index("AccountId", Name = "UQ__Cart__B19E45E8B9479A02", IsUnique = true)]
public partial class Cart
{
    [Key]
    [Column("Cart_Id")]
    public int CartId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Status_Cart")]
    [StringLength(50)]
    public string? StatusCart { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Updated_At", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Cart")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
