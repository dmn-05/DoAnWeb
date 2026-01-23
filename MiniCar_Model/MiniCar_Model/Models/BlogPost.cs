using System.ComponentModel.DataAnnotations.Schema;
namespace MiniCar_Model.Models
{
	[Table("BlogPost")]
  public class BlogPost
  {
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Slug { get; set; } = null!;

    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? Thumbnail { get; set; }

    public string Status { get; set; } = "ACTIVE";

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }

}

