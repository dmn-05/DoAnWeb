using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCar_Model.Models
{
	[Table("ProductPost")]
	public class ProductPost
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public string Summary { get; set; }
		public string Content { get; set; }
		public string Thumbnail { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; }
	}

}
