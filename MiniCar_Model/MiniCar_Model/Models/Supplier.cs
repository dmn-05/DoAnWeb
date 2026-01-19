using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string NameSupplier { get; set; } = null!;

    public string? AddressSupplier { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? StatusSupplier { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
