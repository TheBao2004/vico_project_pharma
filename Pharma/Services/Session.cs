
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Pharma.Services{

    public class Session{

        public IHttpContextAccessor _accessor {set; get;}

        public Session(IHttpContextAccessor accessor){

            _accessor = accessor;

        }

        // public static void RemoveSession(string key){
        //     _accessor.HttpContext.Session.Remove(key);
        // }

        public void SetSessionInt32(string key, int value){
            _accessor.HttpContext.Session.SetInt32(key, value);
        }

        public int GetSessionInt32(string key){
            int value = _accessor.HttpContext.Session.GetInt32(key) ?? 0;
            return value;
        }

        public void SetSessionString(string key, string value){
            _accessor.HttpContext.Session.SetString(key, value);
        }

        public string GetSessionString(string key){
            string value = _accessor.HttpContext.Session.GetString(key);
            return value;
        }

        public void SetFlashSessionInt32(string key, int value){
            _accessor.HttpContext.Session.SetInt32(key + "_flash", value);
        }

        public int GetFlashSessionInt32(string key){
            int value = _accessor.HttpContext.Session.GetInt32(key + "_flash") ?? 0;
            _accessor.HttpContext.Session.Remove(key + "_flash");
            return value;
        }


        public void SetFlashSessionString(string key, string value){
            _accessor.HttpContext.Session.SetString(key + "_flash", value);
        }

        public string GetFlashSessionString(string key){
            string value = _accessor.HttpContext.Session.GetString(key + "_flash");
            _accessor.HttpContext.Session.Remove(key + "_flash");
            return value;
        }

    }


}