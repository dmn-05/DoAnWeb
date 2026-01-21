using MiniCar_Model.Models;
namespace MiniCar_Model.Models.ViewModels
{
	public class AboutViewModel
	{
		public CompanyInfo CompanyInfo { get; set; }
		public List<CompanyPolicy> Policies { get; set; }
		// BLOG
		public List<BlogPost> Blogs { get; set; }

		// PRODUCT POST
		public List<ProductPost> ProductPosts { get; set; }

		// PAGING + SEARCH
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public string Keyword { get; set; }
	}
}	
