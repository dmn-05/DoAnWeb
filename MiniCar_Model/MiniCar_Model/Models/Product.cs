using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("Product_Id")]
    public int ProductId { get; set; }

    [Column("Name_Product")]
    [StringLength(150)]
    public string NameProduct { get; set; } = null!;

    public string? Descriptions { get; set; }

    [Column("Category_Id")]
    public int? CategoryId { get; set; }

    [Column("Supplier_Id")]
    public int? SupplierId { get; set; }

    [Column("Trademark_Id")]
    public int? TrademarkId { get; set; }

    [Column("Promotion_Id")]
    public int? PromotionId { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column("Status_Product")]
    [StringLength(50)]
    public string? StatusProduct { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    [ForeignKey("PromotionId")]
    [InverseProperty("Products")]
    public virtual Promotion? Promotion { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier? Supplier { get; set; }

    [ForeignKey("TrademarkId")]
    [InverseProperty("Products")]
    public virtual Trademark? Trademark { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
