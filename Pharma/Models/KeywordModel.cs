using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharma.Models
{
	[Table("keywords")]
	public class KeywordModel
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Tên"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Name { get; set; }

		[Display(Name = "Từ khóa"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Keyword { get; set; }

		[Display(Name = "Từ khóa"), Required(ErrorMessage = "Vui lòng chọn")]
		public int CategoryId { get; set; }

		[Display(Name = "Kích hoạt")]
		public bool Active { get; set; }

		public CategoryModel Category { get; set; }







	}
}
