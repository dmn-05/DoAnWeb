using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class ProductVariant
{
    public int VariantId { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int ColorId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? StatusVariant { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Color Color { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();

    public virtual Size Size { get; set; } = null!;

    public virtual ICollection<Slideshow> Slideshows { get; set; } = new List<Slideshow>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
