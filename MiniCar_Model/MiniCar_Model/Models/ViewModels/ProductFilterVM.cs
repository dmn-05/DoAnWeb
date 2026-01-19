namespace MiniCar_Model.Models.ViewModels {
  public class ProductFilterVM {
    public List<Size> Sizes { get; set; }
    public List<Trademark> Trademarks { get; set; }
    public List<Color> Colors { get; set; }

    public List<ProductCardVM> Products { get; set; }

  }
}
