using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public string Content { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public string? PostType { get; set; }

    public int? PostCategoryId { get; set; }

    public int? ProductId { get; set; }

    public int AuthorId { get; set; }

    public int? ViewCount { get; set; }

    public string? StatusPost { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Account Author { get; set; } = null!;

    public virtual PostCategory? PostCategory { get; set; }

    public virtual Product? Product { get; set; }
}
