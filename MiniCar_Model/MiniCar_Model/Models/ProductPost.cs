using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class ProductPost
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public string? Thumbnail { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
