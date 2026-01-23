namespace MiniCar_Model.Models.ViewModels
{
	public class RevenueFilterViewModel
	{
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public int? Year { get; set; }

		public List<RevenueStatisticViewModel> Data { get; set; } = new();
	}
}
