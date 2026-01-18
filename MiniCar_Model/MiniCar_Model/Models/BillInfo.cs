using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class BillInfo
{
    public int BillId { get; set; }

    public int VariantId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual ProductVariant Variant { get; set; } = null!;
}
