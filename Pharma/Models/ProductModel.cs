using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{
    [Table("products")]
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên"), Required(ErrorMessage = "Vui lòng điền")]
        public string Name { get; set; }

		[Display(Name = "Slug"), Required(ErrorMessage = "Vui lòng điền")]
		public string Slug { get; set; }

		[Display(Name = "Ảnh")]
        public string? Images { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool Active { get; set; }

        [Display(Name = "Số lượng"), Required(ErrorMessage = "Vui lòng điền")]
        public int Quantity { get; set; }

        [Display(Name = "Giá"), Required(ErrorMessage = "Vui lòng điền")]
        public int Price { get; set; }

        [Display(Name = "Giảm giá")]
        public int? Discount { get; set; }

        [Display(Name = "Mô tả ngắn"), Required(ErrorMessage = "Vui lòng điền")]
        public string Description { get; set; }

        [Display(Name = "Thành phần"), Required(ErrorMessage = "Vui lòng điền")]
        public string Ingredient { get; set; }

        [Display(Name = "Hưỡng dẫn"), Required(ErrorMessage = "Vui lòng điền")]
        public string Guide { get; set; }

        [Display(Name = "Lưu ý"), Required(ErrorMessage = "Vui lòng điền")]
        public string Note { get; set; }

        [Display(Name = "Bảo quản"), Required(ErrorMessage = "Vui lòng điền")]
        public string Preserve { get; set; }

        [Display(Name = "Đóng gói"), Required(ErrorMessage = "Vui lòng điền")]
        public string Pack { get; set; }

        [Display(Name = "Danh mục"), Required(ErrorMessage = "Vui lòng chọn")]
        public int CategoryId { get; set; }

        [Display(Name = "Thương hiệu"), Required(ErrorMessage = "Vui lòng chọn")]
        public int BrandId { get; set; }

        [Display(Name = "Từ khóa"), Required(ErrorMessage = "Vui lòng chọn")]
        public string Keywords { get; set; }



        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }


    }
}
