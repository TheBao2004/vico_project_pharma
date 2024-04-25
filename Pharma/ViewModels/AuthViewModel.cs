

using System.ComponentModel.DataAnnotations;

namespace Pharma.ViewModels{

    public class PageLoginViewModel{

        [Display(Name = "Email"), Required(ErrorMessage = "Vui lòng nhập"), EmailAddress(ErrorMessage = "Email chưa đúng định dạng")]
        public string Email {set; get;}

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Vui lòng nhập")]
        public string Password {set; get;}

    }   

 public class PageRegisterViewModel{

        [Display(Name = "Email"), Required(ErrorMessage = "Vui lòng nhập"), EmailAddress(ErrorMessage = "Email chưa đúng định dạng")]
        public string Email {set; get;}

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Vui lòng nhập")]
        public string Password {set; get;}

        [Display(Name = "Xác nhận"), Required(ErrorMessage = "Vui lòng nhập")]
        public string Confirm {set; get;}

        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Vui lòng nhập")]
        public string Fullname {set; get;}

    }  

}