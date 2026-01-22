namespace MiniCar_Model.Models.ViewModels {
  public class HomeVM {
    public List<ProductCardVM> LatestProducts { get; set; }
    public List<ProductCardVM> DiscountProducts { get; set; }

    public List<Slideshow> Slideshows { get; set; }
  }
}
