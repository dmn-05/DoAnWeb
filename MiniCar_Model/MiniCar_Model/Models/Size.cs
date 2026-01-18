using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public string? Scale { get; set; }

    public string? Descriptions { get; set; }

    public string? StatusSize { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
