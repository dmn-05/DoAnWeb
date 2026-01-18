using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public int VariantId { get; set; }

    public string? UrlImage { get; set; }

    public bool? IsMain { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ProductVariant Variant { get; set; } = null!;
}
