using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{

    [Table("addresss")]
    public class AddressModel
    {

        [Key]
        public int Id { get; set; }

        public int UserId {  get; set; } 

        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Vui lòng điền")]
        public string Name { get; set; }

        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Vui lòng điền")]
        public string Phone { get; set; }

        [Display(Name = "Công ty"), Required(ErrorMessage = "Vui lòng điền")]
        public string Company { get; set; }

        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Vui lòng điền")]
        public string Address { get; set; }

        [Display(Name = "Quốc gia"), Required(ErrorMessage = "Vui lòng chọn")]
        public int NationId { get; set; }

        [Display(Name = "Tỉnh thành"), Required(ErrorMessage = "Vui lòng chọn")]
        public int CityId { get; set; }

        [Display(Name = "Quận huyện"), Required(ErrorMessage = "Vui lòng chọn")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường xã"), Required(ErrorMessage = "Vui lòng chọn")]
        public int WardId { get; set; }

        [Display(Name = "Đặt làm địa chỉ mặc định")]
        public bool Default { get; set; }

        public UserModel User { get; set; }



    }
}
