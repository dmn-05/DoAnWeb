namespace MiniCar_Model.Models.ViewModels
{
  public class CommentIndexVM
  {
    public int CommentId { get; set; }

    public string AccountName { get; set; } = null!;

    public int VariantId { get; set; }

    public double? Rating { get; set; }

    public string? Content { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? StatusComment { get; set; }
  }
}
