namespace MiniCar_Model.Models.ViewModels {
  public class ProductCheckoutVM {
    public int VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public string ColorName {  get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal? PriceTotal { get; set; }
  }
}
