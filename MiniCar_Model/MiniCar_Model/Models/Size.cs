using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Size")]
public partial class Size
{
    [Key]
    [Column("Size_Id")]
    public int SizeId { get; set; }

    [StringLength(50)]
    public string? Scale { get; set; }

    [StringLength(255)]
    public string? Descriptions { get; set; }

    [Column("Status_Size")]
    [StringLength(50)]
    public string? StatusSize { get; set; }

    [InverseProperty("Size")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
