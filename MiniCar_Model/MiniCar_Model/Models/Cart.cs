using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int AccountId { get; set; }

    public string? StatusCart { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
