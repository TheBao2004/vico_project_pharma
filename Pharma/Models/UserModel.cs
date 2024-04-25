using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharma.Models
{
	[Table("users")]
	public class UserModel
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Họ và tên"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Fullname { get; set; }

		[Display(Name = "Email"), Required(ErrorMessage = "Vui lòng nhập"), EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
		public string Email { get; set; }

		[Display(Name = "Mật khẩu"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Password { get; set; }

		public int Permission {  get; set; }

	}
}
