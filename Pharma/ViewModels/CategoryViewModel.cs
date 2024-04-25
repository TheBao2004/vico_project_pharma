using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Pharma.Models;

namespace Pharma.ViewModels
{
	public class AdminCategoryViewModel
	{
	
		public CategoryModel Category { get; set; }

		public List<CategoryModel> Categories { get; set; }

	}

	public class AdminListCategoryViewModel
	{

		public CategoryModel Category { get; set; }
		public List <CategoryModel> Categories { get; set; }

	}


	public class PageCategoryClient{
		public List<ItemProduct> Products {set; get;}
		public List<ItemCategoryClient> Categories {set; get;}
		public CategoryModel? Category {set; get;}	 	

	}

	public class ItemCategoryClient{

		public CategoryModel category {set; get;}
		public int NumberProduct {set; get;}

	}



}
