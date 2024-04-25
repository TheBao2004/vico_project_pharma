using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Pharma.Services{

    public class PhoneViewNam : ValidationAttribute{

        public PhoneViewNam() => ErrorMessage = "{0} phải là số điện thoại việt nam";

        public override bool IsValid(object value)
        {
            if(value == null) return false;
            string pattern = @"^0[0-9]{9}$";    
            return Regex.IsMatch(value.ToString(), pattern);
        }

    }


    public class CheckLikeOther : ValidationAttribute{

        public string _check {set; get;}

        public CheckLikeOther(string check){

            ErrorMessage = "Vui lòng xác nhận lại";
            _check = check;

        }

        public override bool IsValid(object value)
        {
            if(value == null) return false;
            // return !Convert.ToString(value).Equals(_check);   
            return false;
        }


    }

}