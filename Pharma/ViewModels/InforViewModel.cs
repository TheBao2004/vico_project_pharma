

using System.ComponentModel.DataAnnotations;
using Pharma.Models;
using Pharma.Services;

namespace Pharma.ViewModels{

    public class PageInfoViewModel{

        public UserModel User {set; get;}

    }

    public class PageChangePassword{

        [Display(Name = "Mật khẩu cũ"), Required(ErrorMessage = "Vui lòng nhập")]
        public string PassOld {set; get;}
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Vui lòng nhập"), StringLength(20, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
        public string Password {set; get;}
        [Display(Name = "Xác nhận mật khẩu"), Required(ErrorMessage = "Vui lòng nhập"), Compare("Password", ErrorMessage = "Vui lòng xác nhận lại mật khẩu")]
        public string Confirm {set; get;}

    }


    public class PageAddress{

        public List<CityModel> Cities {set; get;}
        public List<DistrictModel> Districts {set; get;}
        public List<WardModel> Wards {set; get;}
        public AddressModel Address {set; get;}
        public List<ItemAddressViewModel> Addresss {set; get;}

    }

    public class ItemAddressViewModel{
        public AddressModel Address {set; get;}
        public CityModel City {set; get;}
        public DistrictModel District {set; get;}
        public WardModel Ward {set; get;}
    }

    public class PageMenuInforViewModel{

        public UserModel User {set; get;}
        public int AddressCount {set; get;}

    }


    public class ItemOrderInfor{

        public OrderModel Order {set; get;}
        public CityModel City {set; get;}
        public DistrictModel District {set; get;}
        public WardModel Ward {set; get;}

    }

    public class PageOrderInfor{

        public List<ItemOrderInfor> Orders {set; get;}

    }


    public class PageOrderDetailInfor{

        public List<DetailOrderModel> Products {set; get;}
        public OrderModel Order {set; get;}
        public CityModel City {set; get;}
        public DistrictModel District {set; get;}
        public WardModel Ward {set; get;}

    }

}