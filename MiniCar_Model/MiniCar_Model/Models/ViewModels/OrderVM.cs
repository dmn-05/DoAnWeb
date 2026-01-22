namespace MiniCar_Model.Models.ViewModels
{
  public class OrderVM
  {
    public int BillId { get; set; }
    public decimal? TotalPrice { get; set; }
    public DateTime? CreateAt { get; set; }
    public string StatusBill { get; set; }

    // Đây là cầu nối quan trọng nhất: Một đơn hàng có danh sách nhiều món hàng
    public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
  }
}
