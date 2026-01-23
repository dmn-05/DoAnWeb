namespace MiniCar_Model.Models.ViewModels {
  public class CheckoutVM {
    public List<ProductCheckoutVM> Products { get; set; } = new List<ProductCheckoutVM>();

    public int CartId { get; set; }

    public Decimal Total { get; set; }

    public Account Customer { get; set; } = new Account();
  }
}
