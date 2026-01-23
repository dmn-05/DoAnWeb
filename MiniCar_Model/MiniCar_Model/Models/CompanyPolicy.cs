using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCar_Model.Models
{
	[Table("CompanyPolicy")]
	public class CompanyPolicy
	{
		public int Id { get; set; }

		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Icon { get; set; }  

		public string? Code { get; set; }
		public int? DisplayOrder { get; set; }
		public string? Status { get; set; } 
	}
}
