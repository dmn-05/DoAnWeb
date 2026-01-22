using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public int AccountId { get; set; }

    public int ProductVariantId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;

}
