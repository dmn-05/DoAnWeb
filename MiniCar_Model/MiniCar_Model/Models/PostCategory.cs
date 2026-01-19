using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class PostCategory
{
    public int PostCategoryId { get; set; }

    public int? ParentId { get; set; }

    public string NameCategory { get; set; } = null!;

    public string? Slug { get; set; }

    public string? StatusCategory { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<PostCategory> InverseParent { get; set; } = new List<PostCategory>();

    public virtual PostCategory? Parent { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
