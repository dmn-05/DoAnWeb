namespace MiniCar_Model.Models.ViewModels
{
  public class WishlistItemVM
  {
    public int WishlistId { get; set; }
    public int VariantId { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Descriptions { get; set; }
  }
}
