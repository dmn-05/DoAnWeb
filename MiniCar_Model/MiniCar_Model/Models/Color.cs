using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Color")]
public partial class Color
{
    [Key]
    [Column("Color_Id")]
    public int ColorId { get; set; }

    [Column("Color_Name")]
    [StringLength(50)]
    public string ColorName { get; set; } = null!;

    [Column("Color_Code")]
    [StringLength(20)]
    public string? ColorCode { get; set; }

    [Column("Status_Color")]
    [StringLength(50)]
    public string? StatusColor { get; set; }

    [InverseProperty("Color")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
