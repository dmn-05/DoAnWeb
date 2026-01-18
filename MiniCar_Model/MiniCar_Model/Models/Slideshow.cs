using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Slideshow
{
    public int SlideshowId { get; set; }

    public string? Title { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? LinkType { get; set; }

    public int? LinkId { get; set; }

    public string? LinkUrl { get; set; }

    public string? Position { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? StatusSlideshow { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
