using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int AccountId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? StatusBill { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
