using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Wishlist")]
public partial class Wishlist
{
  [Key]
  [Column("Wishlist_Id")]
  public int WishlistId { get; set; }

  [Column("Account_Id")]
  public int AccountId { get; set; }

  [Column("ProductVariant_Id")]
  public int ProductVariantId { get; set; }

  [Column("Created_At", TypeName = "datetime")]
  public DateTime? CreatedAt { get; set; }

  [ForeignKey("AccountId")]
  [InverseProperty("Wishlists")]
  public virtual Account Account { get; set; } = null!;

  [ForeignKey("ProductVariantId")]
  [InverseProperty("Wishlists")]
  public virtual ProductVariant ProductVariant { get; set; } = null!;
}
