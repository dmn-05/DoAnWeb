using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int AccountId { get; set; }

    public int VariantId { get; set; }

    public double? Rating { get; set; }

    public string? Content { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? StatusComment { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ProductVariant Variant { get; set; } = null!;
}
