using System.ComponentModel.DataAnnotations;
using Pharma.Models;

namespace Pharma.ViewModels
{
	public class AdminCreateProductViewModel
	{

		public ProductModel Product { get; set; }

		public List<CategoryModel> Categories { get; set; }	
		public List<KeywordModel> Keywords { get; set; }	
	
		public string[] arrKeyword { get; set; }

		public List<BrandModel> Brands { get; set; }

		[Display(Name = "Giá"), Required(ErrorMessage = "Vui lòng điền")]
		public string Price { get; set; }

		[Display(Name = "Khuyễn mãi")]
		public string? Discount { get; set; }

		[Display(Name = "Số lượng"), Required(ErrorMessage = "Vui lòng điền")]
		public string Quantity { get; set; }

	}



	public class PageProductViewModel{

		public ProductModel Product {set; get;}
		public List<ProductModel> ProductCategory {set; get;}
		public List<VoucherModel> Vouchers {set; get;}
		public List<ItemCommentDetail> Comments {set; get;}
		public int CommentId {set; get;}

	}

	public class ItemCommentDetail{

		public CommentModel Comment {set; get;}

		public List<CommentModel> Comments {set; get;}

	}





}
