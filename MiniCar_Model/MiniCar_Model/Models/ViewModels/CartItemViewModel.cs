namespace MiniCar_Model.Models.ViewModels
{
	public class CartItemViewModel
	{
		public int CartItemId { get; set; }
		public int VariantId { get; set; }

		public string ProductName { get; set; }
		public string ColorName { get; set; }
		public string ImageUrl { get; set; }

		public decimal Price { get; set; }
		public int Quantity { get; set; }

		public decimal TotalPrice => Price * Quantity;
	}

}
