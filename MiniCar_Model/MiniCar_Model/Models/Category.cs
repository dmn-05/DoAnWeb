using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    public string NameCategory { get; set; } = null!;

    public string? StatusCategory { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
