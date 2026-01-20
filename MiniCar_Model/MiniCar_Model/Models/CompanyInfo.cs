using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCar_Model.Models
{
	[Table("CompanyInfo")]
	public class CompanyInfo
	{
		[Key]
		public int Id { get; set; }

		public string CompanyName { get; set; }
		public string BusinessField { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Hotline { get; set; }
		public string Description { get; set; }

		public DateTime Updated_At { get; set; }
	}
}
