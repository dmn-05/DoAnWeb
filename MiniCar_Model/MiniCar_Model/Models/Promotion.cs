using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public decimal DiscountValue { get; set; }

    public string? DiscountType { get; set; }

    public string? DescriptionPromotion { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? StatusPromotion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
