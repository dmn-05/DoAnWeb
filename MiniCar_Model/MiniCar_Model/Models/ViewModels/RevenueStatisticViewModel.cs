namespace MiniCar_Model.Models.ViewModels
{
	public class RevenueStatisticViewModel
	{
		public string Label { get; set; }   
		public decimal Revenue { get; set; } // doanh thu
		public int OrderCount { get; set; } // lượt mua

		public DateTime DateKey { get; set; }
	}
}
