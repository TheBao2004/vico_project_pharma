using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{
	[Table("orders")]
	public partial class OrderModel
	{
		[Key]
		public int OrderId { get; set; }
		[Display(Name = "Tên")]
		public string Username { get; set; }

		[Display(Name = "Địa chỉ")]
		public string Address { get; set; }

		[Display(Name = "Tỉnh thành")]
		public int CityId { get; set; }
		//public int UserId { get; set; }

		[Display(Name = "Quận huyện")]
		public int DistrictId { get; set; }

		[Display(Name = "Phường xã")]
		public int WardId { get; set; }

		[Display(Name = "Phương thức thanh toán")]
		public int Payment { get; set; }

		[Display(Name = "Trạng thái")]
		public int State { get; set; }

		[Display(Name = "Số điện thoại")]
		public string Phone { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Ngày đặt")]
		public System.DateTime OrderDate { get; set; }

		[Display(Name = "Thanh toán")]
		public bool Thanhtoan { get; set; } = false;

		[Display(Name = "Ghi chú"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Note {  get; set; } 

		public decimal Total { get; set; }

		public int? UserId { get; set; }

		//public UserModel User { get; set; }

		//public CityModel City { get; set; }
		//public DistrictModel District { get; set; }
		//public WardModel Ward { get; set; }

		public List<DetailOrderModel> DetailOrder { get; set; }
	}
}
