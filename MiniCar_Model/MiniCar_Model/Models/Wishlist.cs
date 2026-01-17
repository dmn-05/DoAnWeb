using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Wishlist")]
[Index("AccountId", "ProductId", Name = "UQ_Wishlist", IsUnique = true)]
public partial class Wishlist
{
    [Key]
    [Column("Wishlist_Id")]
    public int WishlistId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Product_Id")]
    public int ProductId { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Wishlists")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Wishlists")]
    public virtual Product Product { get; set; } = null!;
}
