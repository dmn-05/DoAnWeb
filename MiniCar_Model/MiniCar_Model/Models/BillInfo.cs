using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[PrimaryKey("BillId", "VariantId")]
[Table("BillInfo")]
public partial class BillInfo
{
    [Key]
    [Column("Bill_Id")]
    public int BillId { get; set; }

    [Key]
    [Column("Variant_Id")]
    public int VariantId { get; set; }

    public int Quantity { get; set; }

    [Column("Unit_Price", TypeName = "decimal(12, 2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("BillInfos")]
    public virtual Bill Bill { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("BillInfos")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
