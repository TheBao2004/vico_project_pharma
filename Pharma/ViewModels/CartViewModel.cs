

using System.ComponentModel.DataAnnotations;
using Pharma.Models;

namespace Pharma.ViewModels{

    public class PageCartViewModel{

        public List<CartModel> Carts {set; get;}
        public int Totail {set; get;} 

    }


    public class PageCheckBill{

        public UserModel User {set; get;}        
        public AddressModel Address {set; get;}    
        [Display(Name = "Tỉnh thành"), Required(ErrorMessage = "Vui lòng chọn")]   
        public string City {set; get;}   
        [Display(Name = "Quận huyện"), Required(ErrorMessage = "Vui lòng chọn")]   
        public string District {set; get;}    
        [Display(Name = "Phường xã"), Required(ErrorMessage = "Vui lòng chọn")]   
        public string Ward {set; get;}
        [Display(Name = "Ghi chú"), Required(ErrorMessage = "Vui lòng nhập")]   
        public string Note {set; get;} 
		[Display(Name = "Phương thức thanh toán"), Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
		public int Payment { get; set; }
        public List<ItemCartCheckBill> Carts {set; get;}
        public List<CityModel> Cities {set; get;}
        public List<DistrictModel> Districts {set; get;}
        public List<WardModel> Wards {set; get;}
        public int Total {set; get;}
        public string? Voucher {set; get;}

    }   


    public class ItemCartCheckBill{

        public CartModel Cart {set; get;}
        public int Total {set; get;}

    }

}