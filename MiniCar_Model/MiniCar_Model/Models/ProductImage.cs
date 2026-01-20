using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("ProductImage")]
public partial class ProductImage
{
    [Key]
    [Column("Image_Id")]
    public int ImageId { get; set; }

    [Column("Variant_Id")]
    public int VariantId { get; set; }

    [Column("Url_Image")]
    [StringLength(255)]
    public string? UrlImage { get; set; }

    [Column("Is_Main")]
    public bool IsMain { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [ForeignKey("VariantId")]
    [InverseProperty("ProductImages")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
