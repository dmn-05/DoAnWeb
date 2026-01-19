namespace MiniCar_Model.Models.ViewModels {
  public class ProductCardVM {
    public int ProductId { get; set; }
    public string NameProduct { get; set; }

    public decimal Price { get; set; }

    public string ImageUrl { get; set; }
    // OPTIONAL
    public int VariantId { get; set; }
  }
}

