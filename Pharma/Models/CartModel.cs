using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{

    [Table("carts")]
    public class CartModel
    {
		[Key]
		public int Id { get; set; }

		[Display(Name = "Mã đơn hàng")]
		public string CartId { get; set; }

		[Display(Name = "Mã sản phẩm")]
		public int ProductId { get; set; }

		[Display(Name = "Số lượng")]
		public int Count { get; set; }

		[Display(Name = "Ngày đặt")]
		public System.DateTime DateCreated { get; set; }

		public virtual ProductModel Product { get; set; }

	}
}
