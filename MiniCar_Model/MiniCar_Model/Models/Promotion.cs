using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Promotion")]
public partial class Promotion
{
    [Key]
    [Column("Promotion_Id")]
    public int PromotionId { get; set; }

    [Column("Discount_Value", TypeName = "decimal(10, 2)")]
    public decimal DiscountValue { get; set; }

    [Column("Discount_Type")]
    [StringLength(10)]
    public string? DiscountType { get; set; }

    [Column("Start_Date", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("End_Date", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column("Status_Promotion")]
    [StringLength(50)]
    public string? StatusPromotion { get; set; }

    [InverseProperty("Promotion")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
