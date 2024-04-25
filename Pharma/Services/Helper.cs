using System.IO;
using System.Text.Json;
using Pharma.Data;

namespace Pharma.Services
{
    public class Helper
    {

        public readonly AppDbContext _context;
        public IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        private string _toUpload { get; set; }

        public Helper(IHttpContextAccessor accessor, AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _accessor = accessor;
            _env = env;
            _toUpload = Path.Combine(env.WebRootPath, "uploads");
        }

        public async Task<string> Upload(IFormFile file, string? old = "")
        {

            string nameImg = Guid.NewGuid().ToString() + "_" + file.FileName.ToString();

            string filePath = Path.Combine(_toUpload, nameImg);

            using (var img = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(img);
            }

            if (!string.IsNullOrEmpty(old))
            {
                if (File.Exists(Path.Combine(_toUpload, old)))
                {
                    File.Delete(Path.Combine(_toUpload, old));
                }
            }

            return nameImg;

        }


        public void RemoveImage(string img)
        {
            if (File.Exists(Path.Combine(_toUpload, img)))
            {
                File.Delete(Path.Combine(_toUpload, img));
            }
        }



        public async Task<string> UploadMuch(IFormFileCollection files)
        {

            var number = files.Count;

            List<string> res = new List<string>();

            string img = "";

            if (number > 0)
            {
                foreach (var file in files)
                {
                    string nameImage = Guid.NewGuid().ToString() + "_" + file.FileName.ToString();
                    img = nameImage;
                    res.Add(nameImage);

                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(_toUpload, nameImage);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
            }

            return img;

        }

        public static int PhanTramGiam(int Discount, int Price)
        {

            int Minus = Price - Discount;
            decimal ChiaHieu = (decimal)Minus / (decimal)Price;
            decimal Res = ChiaHieu * 100;
            // Console.WriteLine(Price);
            // Console.WriteLine(Minus);
            // Console.WriteLine(ChiaHieu);
            // Console.WriteLine(Res);
            return (int)Res;

        }

        public static string[] StringToArray(string str, string kytu)
        {

            string[] arrStr = str.Split(kytu);

            arrStr = arrStr.Where(x => x != "").ToArray();

            if (arrStr.Count() < 1) return new string[] { "" };

            return arrStr;

        }

        public static string Image(string str, string kytu)
        {

            string[] arrStr = str.Split(kytu);

            arrStr = arrStr.Where(x => x != "").ToArray();

            return arrStr[0];

        }

        public static string ImageDefault(string value)
        {

            if (string.IsNullOrEmpty(value)) return "warning.jpg";
            return value;

        }

        public bool IsInt(string x)
        {

            int number;
            if (int.TryParse(x, out number))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IsLogin()
        {
            if (_accessor.HttpContext.Session.GetInt32("UserId") == null) return false;
            return true;
        }

        public static string IfOrElse(int IfStr, int ElseStr)
        {
            // if(string.IsNullOrEmpty(Convert.ToString(IfStr))) return Convert.ToString(ElseStr);
            if (Convert.ToString(IfStr) == "0") return Convert.ToString(ElseStr);
            return Convert.ToString(IfStr);
        }

        public static int CalculateTotal(int x, int y)
        {
            return x - y;
        }

        public static int CalculatePhanTram(int x, int y)
        {
            // Tính giá trị phần trăm của số
            var giáTrịPhầnTrăm = (x * y) / 100;

            // Trả về số còn lại sau khi trừ phần trăm
            return x - giáTrịPhầnTrăm;
        }

    }
}
