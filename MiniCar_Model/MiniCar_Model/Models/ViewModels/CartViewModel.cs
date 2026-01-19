namespace MiniCar_Model.Models.ViewModels
{
	public class CartViewModel
	{
		public List<CartItemViewModel> Items { get; set; } = new();

		public int TotalQuantity => Items.Sum(x => x.Quantity);
		public decimal TotalAmount => Items.Sum(x => x.TotalPrice);
	}
}
