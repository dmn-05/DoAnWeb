using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("CartItem")]
[Index("CartId", "VariantId", Name = "UQ_Cart_Variant", IsUnique = true)]
public partial class CartItem
{
    [Key]
    [Column("CartItem_Id")]
    public int CartItemId { get; set; }

    [Column("Cart_Id")]
    public int CartId { get; set; }

    [Column("Variant_Id")]
    public int VariantId { get; set; }

    public int Quantity { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("CartItems")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
