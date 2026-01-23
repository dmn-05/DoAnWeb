namespace MiniCar_Model.Models.ViewModels
{
  public class ProductIndexVM
  {
    public int ProductId { get; set; }
    public string NameProduct { get; set; } = null!;
    public string? CategoryName { get; set; }

    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }

    public int TotalQuantity { get; set; }
    public int VariantCount { get; set; }

    public string? StatusProduct { get; set; }
  }
}
