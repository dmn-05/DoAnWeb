using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Bill")]
public partial class Bill
{
    [Key]
    [Column("Bill_Id")]
    public int BillId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Total_Price", TypeName = "decimal(12, 2)")]
    public decimal? TotalPrice { get; set; }

    [Column("Payment_Date", TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [Column("Status_Bill")]
    [StringLength(50)]
    public string? StatusBill { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Bills")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("Bill")]
    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
