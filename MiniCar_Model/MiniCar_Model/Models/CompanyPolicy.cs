using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCar_Model.Models
{
	public class CompanyPolicy
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		[StringLength(300)]
		public string Description { get; set; }

		public string Icon { get; set; }   // đường dẫn ảnh

		public int DisplayOrder { get; set; }

		public string Status { get; set; } = "ACTIVE";
	}
}
