using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{
    [Table("brands")]
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên"), Required(ErrorMessage = "Vui lòng điền")]
        public string Name { get; set; }

		[Display(Name = "Slug"), Required(ErrorMessage = "Vui lòng điền")]
		public string Slug { get; set; }

		[Display(Name = "Ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool Active { get; set; } = false;
    }
}
