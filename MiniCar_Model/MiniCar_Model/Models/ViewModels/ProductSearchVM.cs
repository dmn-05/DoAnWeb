namespace MiniCar_Model.Models.ViewModels {
  public class ProductSearchVM {
    public string? Keyword { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public List<ProductVariant>? Results { get; set; }
  }
}
