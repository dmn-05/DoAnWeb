using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string NameProduct { get; set; } = null!;

    public string? Descriptions { get; set; }

    public int? CategoryId { get; set; }

    public int? SupplierId { get; set; }

    public int? TrademarkId { get; set; }

    public int? PromotionId { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? StatusProduct { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public virtual Promotion? Promotion { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual Trademark? Trademark { get; set; }
}
