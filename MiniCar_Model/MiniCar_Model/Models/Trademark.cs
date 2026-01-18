using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Trademark
{
    public int TrademarkId { get; set; }

    public string NameTrademark { get; set; } = null!;

    public string? StatusTrademark { get; set; }

    public string? Descriptions { get; set; }

    public string? Country { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
