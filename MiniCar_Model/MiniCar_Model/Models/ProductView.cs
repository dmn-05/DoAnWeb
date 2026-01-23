using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class ProductView
{
    public int ViewId { get; set; }

    public int VariantId { get; set; }

    public DateTime? ViewDate { get; set; }

    public virtual ProductVariant Variant { get; set; } = null!;
}
