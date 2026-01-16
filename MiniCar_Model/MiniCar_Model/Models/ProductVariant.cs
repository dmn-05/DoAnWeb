using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("ProductVariant")]
[Index("ProductId", "SizeId", "ColorId", Name = "UQ_Product_Size_Color", IsUnique = true)]
public partial class ProductVariant
{
    [Key]
    [Column("Variant_Id")]
    public int VariantId { get; set; }

    [Column("Product_Id")]
    public int ProductId { get; set; }

    [Column("Size_Id")]
    public int SizeId { get; set; }

    [Column("Color_Id")]
    public int ColorId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Column("Status_Variant")]
    [StringLength(50)]
    public string? StatusVariant { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Variant")]
    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();

    [InverseProperty("Variant")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("ColorId")]
    [InverseProperty("ProductVariants")]
    public virtual Color Color { get; set; } = null!;

    [InverseProperty("Variant")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductVariants")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("Variant")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [ForeignKey("SizeId")]
    [InverseProperty("ProductVariants")]
    public virtual Size Size { get; set; } = null!;
}
