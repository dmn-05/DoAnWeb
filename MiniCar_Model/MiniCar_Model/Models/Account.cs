using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Account")]
[Index("Email", Name = "UQ__Account__A9D10534016D8F73", IsUnique = true)]
[Index("UserName", Name = "UQ__Account__C9F28456B0FA2B64", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Role_Id")]
    public int RoleId { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [Column("Name_Account")]
    [StringLength(150)]
    public string? NameAccount { get; set; }

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [Column("Address_Account")]
    [StringLength(255)]
    public string? AddressAccount { get; set; }

    [Column("Phone_Number")]
    [StringLength(10)]
    public string? PhoneNumber { get; set; }

    [Column("Password_Account")]
    [StringLength(255)]
    public string PasswordAccount { get; set; } = null!;

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column("Status_Account")]
    [StringLength(50)]
    public string? StatusAccount { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [InverseProperty("Account")]
    public virtual Cart? Cart { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("RoleId")]
    [InverseProperty("Accounts")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("Account")]
    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();

    [InverseProperty("Account")]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    [InverseProperty("Account")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
