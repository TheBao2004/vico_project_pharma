using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{
	[Table("orderDetails")]
	public class DetailOrderModel
	{
		[Key]
		public int OrderDetailId { get; set; }

		[Display(Name = "Mã đơn")]
		public int OrderId { get; set; }

		[Display(Name = "Mã sản phẩm")]
		public int ProductId { get; set; }

		[Display(Name = "Số lượng")]
		public decimal Price { get; set; }

		public int Quantity { get; set; }

		[Display(Name = "Đơn vị giá")]
		public virtual ProductModel Product { get; set; }

		public virtual OrderModel Order { get; set; }
	}
}
