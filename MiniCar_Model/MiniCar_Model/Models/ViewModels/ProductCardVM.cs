namespace MiniCar_Model.Models.ViewModels {
  public class ProductCardVM {
    public int ProductId { get; set; }
    public string NameProduct { get; set; } = string.Empty;

    public string NameCategory { get; set; } = null!;
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    // OPTIONAL
    public int VariantId { get; set; }

  }
}

