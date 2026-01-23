using MiniCar_Model.Models;
namespace MiniCar_Model.Models.ViewModels
{
	public class AboutViewModel
	{
		public CompanyInfo CompanyInfo { get; set; }
		public List<CompanyPolicy> Policies { get; set; }
		// BLOG
		public List<BlogPost> Blogs { get; set; }
		public int BlogCurrentPage { get; set; }
		public int BlogTotalPages { get; set; }

		// PRODUCT
		public List<ProductPost> ProductPosts { get; set; }
		public int ProductCurrentPage { get; set; }
		public int ProductTotalPages { get; set; }

		public string Keyword { get; set; }
	}
}	
