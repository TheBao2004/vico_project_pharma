using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Pharma.Models
{
	[Table("comments")]
	public class CommentModel
	{
		[Key]
		public int Id { get; set; }
		
		public int UserId { get; set; }
		public int ParentId { get; set; }

		public int ProductId { set; get; }

		public string Comment { get; set; }
		public bool Active { get; set; } = false;

		public DateTime Created { get; set; }

		public ProductModel Product { get; set; }
		public UserModel User { get; set; }

	}
}
