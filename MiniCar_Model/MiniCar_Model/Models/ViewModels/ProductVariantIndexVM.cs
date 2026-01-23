namespace MiniCar_Model.Models.ViewModels
{
  public class ProductVariantIndexVM
  {
    public int VariantId { get; set; }

    public string SizeName { get; set; } = null!;
    public string ColorName { get; set; } = null!;

    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public string StatusVariant { get; set; } = null!;
  }
}
