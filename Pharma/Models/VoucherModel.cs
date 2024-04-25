using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharma.Models
{
	[Table("vouchers")]
	public class VoucherModel
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Kích hoạt")]
		public bool Active { set; get; } = false;

		[Display(Name = "Tên"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Name { set; get; }

		[Display(Name = "Mã khuyễn mãi"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Code { set; get; }

		[Display(Name = "Điều kiện khuyến mãi"), Required(ErrorMessage = "Vui lòng chọn")]
		public int Condition { get; set; }

		[Display(Name = "Chi số điều kiện khuyến mãi"), Required(ErrorMessage = "Vui lòng nhập")]
		public int NumberCondition { get; set; }

		[Display(Name = "Khuyến mãi"), Required(ErrorMessage = "Vui lòng chọn")]
		public int Voucher { get; set; }

		[Display(Name = "Chỉ số khuyến mãi"), Required(ErrorMessage = "Vui lòng nhập")]
		public int NumberVoucher { get; set; }
	}
}
