using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pharma.Models;
using Pharma.Data;
using Pharma.Services;
using Pharma.ViewModels;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Schema;
using Humanizer;
using Azure.Core;
using Newtonsoft.Json;

namespace Pharma.Controllers
{
    public class HomeController : Controller
    {



        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public ShoppingCart _cart { set; get; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _accessor = accessor;
            _helper = new Helper(accessor, context, env);
            _cart = ShoppingCart.GetCart(context, accessor);
        }

        public async Task<IActionResult> Index()
        {

            // for (int i = 0; i < 50; i++)
            // {
            //     _context.ProductModel.Add(new ProductModel{

            //         Name = "Sản phẩm " + i,
            //         Slug = "san-pham-" + i,
            //         Images = "c94fe905-e1ba-4b23-bc0e-7d301058d502_sua-bot-nutifood-varna-elite-bo.jpg,",
            //         Active = true,
            //         Quantity = 12,
            //         Price = 1500000 + i,
            //         Discount = 1300500 + i,
            //         Description = "Mô tả ngắn",
            //         Ingredient = "Thành phần",
            //         Guide = "Hưỡng dẫn",
            //         Note = "Lưu ý",
            //         Preserve = "Bảo quản",
            //         Pack = "Đóng gói",
            //         CategoryId = 13,
            //         BrandId = 3,
            //         Keywords = "nguoi-cao-tuoi,nguoi-tieu-duong,vitamin-tong-hop"

            //     });
            // }

            // await _context.SaveChangesAsync();


            var Categories = _context.CategoryModel.Where(x => x.Active == true).ToList();
            var Products = _context.ProductModel.Where(x => x.Active == true).ToList();

            List<ItemCategoryHome> CategoryModel = new List<ItemCategoryHome>();
            List<ItemProduct> ProductBanner = new List<ItemProduct>();
            List<ItemProduct> ProductForObjects = new List<ItemProduct>();
            List<ItemProduct> ProductSuggests = new List<ItemProduct>();
            List<ItemProduct> ProductDuocPhams = new List<ItemProduct>();
            List<ItemProduct> ProductSuaBot = new List<ItemProduct>();
            List<ItemProduct> ProductSinhLy = new List<ItemProduct>();

            foreach (var cate in Categories)
            {
                CategoryModel.Add(new ItemCategoryHome
                {
                    Category = cate,
                    NumberProduct = _context.ProductModel.Where(x => x.CategoryId == cate.Id && x.Active == true).Count()
                });
            }

            foreach (var pro in Products)
            {
                var Thumbnail = "";

                if (!string.IsNullOrEmpty(pro.Images))
                {
                    Thumbnail = pro.Images.Split(",")[0];
                }
                else
                {
                    Thumbnail = "warning.jpg";
                }


                if (pro.CategoryId == 18)
                {
                    ProductBanner.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });
                }

                if (pro.CategoryId == 13)
                {

                    ProductForObjects.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });

                }


                if (pro.CategoryId == 14)
                {

                    ProductSuaBot.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });

                }

                if (pro.CategoryId == 15)
                {

                    ProductSinhLy.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });

                }

                if (pro.CategoryId == 15)
                {

                    ProductSuggests.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });

                }

                if (pro.CategoryId == 17)
                {


                    ProductDuocPhams.Add(new ItemProduct
                    {
                        Product = pro,
                        Thumbnail = Thumbnail
                    });

                }


            }

            var ProductForObject = new SectionProductForObject()
            {
                Category = _context.CategoryModel.Where(x => x.Id == 13).FirstOrDefault(),
                Keywords = _context.KeywordModel.Where(x => x.CategoryId == 13 && x.Active == true).ToList(),
                Products = ProductForObjects,
            };

            var ProductSuggest = new SectionProductSuggest()
            {
                Category = _context.CategoryModel.Where(x => x.Id == 16).FirstOrDefault(),
                Keywords = _context.KeywordModel.Where(x => x.CategoryId == 16 && x.Active == true).ToList(),
                Products = ProductSuggests,
            };

            var ProductDuocPham = new SectionProductDuocPham()
            {
                Category = _context.CategoryModel.Where(x => x.Id == 17).FirstOrDefault(),
                Keywords = _context.KeywordModel.Where(x => x.CategoryId == 17 && x.Active == true).ToList(),
                Products = ProductDuocPhams,
            };

            PageHomeViewModel model = new PageHomeViewModel
            {
                Categories = CategoryModel,
                ProductBanner = ProductBanner,
                Vouchers = _context.VoucherModel.Where(x => x.Active == true).OrderByDescending(x => x.Id).ToList(),
                ProductForObject = ProductForObject,
                Brands = _context.BrandModel.Where(x => x.Active == true).ToList(),
                ProductSuaBot = ProductSuaBot,
                ProductSinhLy = ProductSinhLy,
                ProductSuggest = ProductSuggest,
                ProductDuocPham = ProductDuocPham
            };

            return View(model);
        }

        [Route("danh-muc/{slug?}")]
        public IActionResult Category(string? slug)
        {

            var Categories = _context.CategoryModel.Where(x => x.Active == true).ToList();
            var Products = _context.ProductModel.Where(x => x.Active == true).ToList();

            List<ItemProduct> ProductModel = new List<ItemProduct>();

            foreach (var pro in Products)
            {
                var Thumbnail = "";

                if (!string.IsNullOrEmpty(pro.Images))
                {
                    Thumbnail = pro.Images.Split(",")[0];
                }
                else
                {
                    Thumbnail = "warning.jpg";
                }

                ProductModel.Add(new ItemProduct
                {
                    Product = pro,
                    Thumbnail = Thumbnail
                });

            }

            List<ItemCategoryClient> CategoryModel = new List<ItemCategoryClient>();

            var model = new PageCategoryClient();

            if (!string.IsNullOrEmpty(slug))
            {
                var Category = Categories.Where(x => x.Slug == slug && x.Active == true).FirstOrDefault();
                if (Category == null) return NotFound();
                model.Category = Category;
                if (Category.CategoryId == 0)
                {
                    var CategoryClient = Categories.Where(x => x.CategoryId == Category.Id).ToList();
                    foreach (var item in CategoryClient)
                    {
                        var ItemCategoryClient = new ItemCategoryClient
                        {
                            category = item,
                            NumberProduct = _context.ProductModel.Where(x => x.CategoryId == item.Id).Count()
                        };
                        // Console.WriteLine($"asdasdadasdasd --  23123: {ItemCategoryClient.category.Name}");
                        CategoryModel.Add(ItemCategoryClient);
                    }
                    model.Categories = CategoryModel.Take(8).ToList();
                }
                else
                {
                    var CategoryClient = Categories.Where(x => x.CategoryId == Category.CategoryId).ToList();
                    foreach (var item in CategoryClient)
                    {
                        CategoryModel.Add(new ItemCategoryClient
                        {
                            category = item,
                            NumberProduct = _context.ProductModel.Where(x => x.CategoryId == item.Id).Count()
                        });
                    }
                    model.Categories = CategoryModel.Take(8).ToList();

                }
                ProductModel = ProductModel.Where(x => x.Product.CategoryId == Category.Id).ToList();
                model.Products = ProductModel;
            }
            else
            {
                model.Products = ProductModel;
                foreach (var item in Categories)
                {
                    // Console.WriteLine(item.Id);
                    var itemCate = new ItemCategoryClient
                    {
                        category = item,
                        NumberProduct = _context.ProductModel.Where(x => x.CategoryId == item.Id).Count()
                        // NumberProduct = 3
                    };
                    CategoryModel.Add(itemCate);
                }
                model.Categories = CategoryModel.Take(8).ToList();
            }

            return View(model);
        }


        [Route("san-pham/{slug}/{parent?}")]
        public IActionResult Detail(string slug, int? parent = null)
        {

            if (string.IsNullOrEmpty(slug)) return NotFound();

            var productModel = _context.ProductModel.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Slug == slug && x.Active == true).FirstOrDefault();

            if (productModel == null) return NotFound();

            var ProductCategory = _context.ProductModel.Where(x => x.CategoryId == productModel.CategoryId).ToList();

            if (productModel == null) return NotFound();

            var comments = _context.CommentModel.Include(x => x.User).Where(x => x.ProductId == productModel.Id && x.Active == true && x.ParentId == 0).ToList();

            var Comments = new List<ItemCommentDetail>();

            foreach (var item in comments)
            {
                Comments.Add(new ItemCommentDetail
                {
                    Comment = item,
                    Comments = _context.CommentModel.Where(x => x.ParentId == item.Id && x.Active == true).ToList()
                });
            }

            var model = new PageProductViewModel
            {
                Product = productModel,
                ProductCategory = ProductCategory,
                Vouchers = _context.VoucherModel.Where(x => x.Active == true).ToList(),
                Comments = Comments
            };

            if (parent != null)
            {
                model.CommentId = parent ?? 0;
                if (_context.CommentModel.Find(parent) == null) return RedirectToAction("Detail", "Home", new { slug = productModel.Slug });
                // if(_context.CommentModel.Find(parent) == null) return RedirectToAction("Category", "Home", new {slug = productModel.Category.Slug});
                // if(_context.CommentModel.Find(parent) == null) return NotFound($"12321 {productModel.Slug}");
            }

            return View(model);
        }


        [Route("dang-nhap")]
        public async Task<IActionResult> Login()
        {
            // if (HttpContext.Session.GetString("UserId") != null) return NotFound();
            if (User.Identity.IsAuthenticated) return RedirectToAction("Infor");

            return View();
        }

        [HttpPost("dang-nhap")]
        public async Task<IActionResult> Login(PageLoginViewModel model)
        {

            var user = _context.UserModel.Where(x => x.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("Email", "Không tồn tại email này");
            }
            else
            {
                // if (user.Password == model.Password)
                // {
                //     HttpContext.Session.SetString("UserId", Convert.ToString(user.Id));
                //     return RedirectToAction("Infor");
                // }
                // else
                // {
                //     ModelState.AddModelError("Password", "Mật khẩu không chính xác");
                // }

                if (user.Password == model.Password)
                {

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Email, user.Email)
                    // add or remove claims as necessary    
                };

                    var claimsIdentity = new ClaimsIdentity(claims, "MyAuthScheme");

                    await _accessor.HttpContext
                        .SignInAsync("MyAuthScheme",
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties());
                    return RedirectToAction("Infor");
                }
                else
                {
                    ModelState.AddModelError("Password", "Mật khẩu không chính xác");
                }

            }

            return View(model);
        }


        [Route("dang-ky")]
        public IActionResult Register()
        {
            // if (HttpContext.Session.GetString("UserId") != null) return NotFound();
            if (User.Identity.IsAuthenticated) return RedirectToAction("Infor");

            return View();
        }


        [HttpPost("dang-ky")]
        public async Task<IActionResult> Register(PageRegisterViewModel model)
        {

            bool error = true;

            if (_context.UserModel.Where(x => x.Email == model.Email).FirstOrDefault() != null)
            {
                ModelState.AddModelError("Email", "Đã có người dùng dùng email này");
                error = false;
            }

            if (model.Password != model.Confirm)
            {
                ModelState.AddModelError("Confirm", "Xác nhận lại mật khẩu");
                error = false;
            }

            if (error)
            {

                var user = new UserModel()
                {
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Password = model.Password,
                    Permission = 0
                };

                _context.Add(user);

                await _context.SaveChangesAsync();

                // HttpContext.Session.SetString("UserId", Convert.ToString(_context.UserModel.Where(x => x.Email == model.Email).FirstOrDefault().Id));

                var userNew = _context.UserModel.Where(x => x.Email == model.Email).FirstOrDefault();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userNew.Id)),
                    new Claim(ClaimTypes.Name, Convert.ToString(userNew.Id)),
                    new Claim(ClaimTypes.Email, userNew.Email)
                    // add or remove claims as necessary    
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyAuthScheme");

                await _accessor.HttpContext
                    .SignInAsync("MyAuthScheme",
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties());

                return RedirectToAction("Infor");

            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {

            if (!User.Identity.IsAuthenticated) return NotFound();

            await _accessor.HttpContext
            .SignOutAsync("MyAuthScheme");

            // HttpContext.Session.Remove("CartSessionKey");

            return RedirectToAction("Login");

        }


        [Route("thong-tin-tai-khoan")]
        public IActionResult Infor()
        {
            // int user_id = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            // if (user_id == 0) return RedirectToAction("Login");

            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login");

            int user_id = int.Parse(User.Identity.Name);

            var user = _context.UserModel.Where(x => x.Id == user_id).FirstOrDefault();

            if (user == null) return NotFound();

            var model = new PageInfoViewModel
            {
                User = user
            };

            return View(model);
        }

        [Route("don-hang")]
        public IActionResult Orders()
        {

            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");

            var user_id = int.Parse(User.Identity.Name);

            var orders = _context.OrderModel.Where(x => x.UserId == user_id).ToList();

            var Orders = new List<ItemOrderInfor>();

            foreach (var o in orders)
            {
                Orders.Add(new ItemOrderInfor
                {
                    Order = o,
                    City = _context.CityModel.Where(x => x.Id == o.CityId).FirstOrDefault(),
                    District = _context.DistrictModel.Where(x => x.Id == o.DistrictId).FirstOrDefault(),
                    Ward = _context.WardModel.Where(x => x.ID == o.WardId).FirstOrDefault(),
                });
            }

            var model = new PageOrderInfor()
            {
                Orders = Orders
            };

            return View(model);
        }

        [Route("chi-tiet-don-hang-{id}")]
        public IActionResult OrderDetail(int id)
        {

            if (id == null) return NotFound();

            var order = _context.OrderModel.Where(x => x.OrderId == id).FirstOrDefault();

            if (order == null) return NotFound();

            var products = _context.DetailOrderModel.Include(x => x.Product).Where(x => x.OrderId == id).ToList();

            var model = new PageOrderDetailInfor()
            {
                Order = order,
                Products = products,
                City = _context.CityModel.Where(x => x.Id == order.CityId).FirstOrDefault(),
                District = _context.DistrictModel.Where(x => x.Id == order.DistrictId).FirstOrDefault(),
                Ward = _context.WardModel.Where(x => x.ID == order.WardId).FirstOrDefault(),
            };

            return View(model);
        }

        [Route("doi-mat-khau")]
        public IActionResult ChangePassword()
        {

            // if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"))) return NotFound();
            if (!User.Identity.IsAuthenticated) return NotFound();

            return View();
        }

        [HttpPost("doi-mat-khau")]
        public async Task<IActionResult> ChangePassword(PageChangePassword model)
        {

            // int UserId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            // if (UserId == 0) return NotFound();

            if (!User.Identity.IsAuthenticated) return NotFound();

            int UserId = int.Parse(User.Identity.Name);

            bool error = true;

            var user = _context.UserModel.Where(x => x.Id == UserId).FirstOrDefault();

            if (model.PassOld != user.Password)
            {
                ModelState.AddModelError("PassOld", "Mật khẩu không đúng");
                error = false;
            }

            if (error)
            {

                user.Password = model.Password;
                _context.Update(user);
                await _context.SaveChangesAsync();

                ViewBag.Success = "Đã đổi mật khẩu thành công";

            }

            return View(model);

        }


        [Route("them-dia-chi")]
        public IActionResult AddAddress()
        {

            if (!User.Identity.IsAuthenticated) return NotFound();

            int user_id = int.Parse(User.Identity.Name);

            var Addresss = _context.AddressModel.Include(x => x.User).Where(x => x.UserId == user_id).ToList();

            List<ItemAddressViewModel> listAddress = new List<ItemAddressViewModel>();

            foreach (var x in Addresss)
            {
                listAddress.Add(new ItemAddressViewModel
                {
                    Address = x,
                    City = _context.CityModel.Where(c => c.Id == x.CityId).FirstOrDefault(),
                    District = _context.DistrictModel.Where(c => c.Id == x.DistrictId).FirstOrDefault(),
                    Ward = _context.WardModel.Where(c => c.ID == x.WardId).FirstOrDefault()
                });
            }

            var model = new PageAddress
            {
                Cities = _context.CityModel.ToList(),
                Districts = _context.DistrictModel.ToList(),
                Wards = _context.WardModel.ToList(),
                Addresss = listAddress
            };

            return View(model);
        }

        [HttpPost("them-dia-chi")]
        public async Task<IActionResult> AddAddress(PageAddress model)
        {


            // var user_id = HttpContext.Session.GetString("UserId") ?? "0";
            int user_id = int.Parse(User.Identity.Name);
            var name = Request.Form["fullname"];
            var phone = Request.Form["phone"];
            var address = Request.Form["address"];
            var city = Request.Form["city"];
            var district = Request.Form["district"];
            var ward = Request.Form["ward"];
            var Default = Request.Form["default"];
            bool macDinh = false;
            if (!string.IsNullOrEmpty(Default)) macDinh = true;

            // var edit = Request.Form["address_id"];
            // return NotFound($"hiihi {name}");

            // if(edit != null){

            if (_context.AddressModel.Where(x => x.UserId == user_id).Count() <= 0)
            {
                macDinh = true;
            }
            else
            {
                if (macDinh)
                {
                    var addresss = _context.AddressModel.Where(x => x.UserId == user_id).ToList();
                    foreach (var sc in addresss)
                    {
                        sc.Default = false;
                        _context.AddressModel.Update(sc);
                    }
                }
            }

            // }

            var addressNew = new AddressModel
            {
                UserId = user_id,
                Name = name,
                Phone = phone,
                Company = "Khong co",
                Address = address,
                NationId = 0,
                CityId = int.Parse(city),
                DistrictId = int.Parse(district),
                WardId = int.Parse(ward),
                Default = macDinh
            };

            // return NotFound($"{user_id} - {name} - {phone} - {macDinh} - {addressNew.Default}");

            _context.Add(addressNew);

            await _context.SaveChangesAsync();

            return RedirectToAction("AddAddress");

        }

        public async Task<IActionResult> InforAddress()
        {
            var id = int.Parse(Request.Form["id"]);

            Console.WriteLine($"asdasasdasdasdasd --- {id}");

            var model = new PageAddress
            {
                Cities = _context.CityModel.ToList(),
                Districts = _context.DistrictModel.ToList(),
                Wards = _context.WardModel.ToList(),
                Address = _context.AddressModel.Where(x => x.Id == id).FirstOrDefault()
            };

            return PartialView("~/Views/Shared/FormAddress.cshtml", model);

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddressInfor(PageAddress model)
        {

            var d = model.Address.Name;

            // await _context.SaveChangesAsync();

            int user_id = int.Parse(User.Identity.Name);
            int id = int.Parse(Request.Form["id_edit"]);
            var name = Request.Form["fullname_edit"];
            var phone = Request.Form["phone_edit"];
            var address = Request.Form["address_edit"];
            var city = int.Parse(Request.Form["city_edit"]);
            var district = int.Parse(Request.Form["district_edit"]);
            var ward = int.Parse(Request.Form["ward_edit"]);
            var Default = Request.Form["default_edit"];
            bool macDinh = false;
            if (!string.IsNullOrEmpty(Default)) macDinh = true;

            if (macDinh)
            {
                var addresss = _context.AddressModel.Where(x => x.UserId == user_id && x.Id != id).ToList();
                foreach (var sc in addresss)
                {
                    sc.Default = false;
                    _context.AddressModel.Update(sc);
                }
            }

            _context.AddressModel.Update(new AddressModel
            {
                Id = id,
                UserId = user_id,
                Name = name,
                Phone = phone,
                Company = "Chưa có",
                Address = address,
                NationId = 0,
                CityId = city,
                DistrictId = district,
                WardId = ward,
                Default = macDinh
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("AddAddress");

            return NotFound($"sdadasds {id} - {district}");

        }

        public async Task<IActionResult> RemoveAddress(int id)
        {

            var address = _context.AddressModel.Where(x => x.Id == id).FirstOrDefault();

            if (address == null) return NotFound();

            _context.AddressModel.Remove(address);

            await _context.SaveChangesAsync();

            return RedirectToAction("AddAddress");

        }


        [Route("thanh-toan")]
        public IActionResult CheckBill()
        {
            if (_cart.GetCount() <= 0) return NotFound();

            var carts = _cart.GetCartItems();

            var Carts = new List<ItemCartCheckBill>();

            foreach (var item in carts)
            {
                var Total = item.Product.Price * item.Count;
                if (item.Product.Discount > 0) Total = (item.Product.Discount ?? 0) * item.Count;

                Carts.Add(new ItemCartCheckBill
                {
                    Cart = item,
                    Total = Total
                });
            }

            var model = new PageCheckBill()
            {
                Cities = _context.CityModel.ToList(),
                Districts = _context.DistrictModel.ToList(),
                Wards = _context.WardModel.ToList(),
                Carts = Carts,
                Total = (int)_cart.GetTotal()
            };


            if (User.Identity.IsAuthenticated)
            {
                int user_id = int.Parse(User.Identity.Name);
                var user = _context.UserModel.Where(x => x.Id == user_id).FirstOrDefault();
                var address = _context.AddressModel.Where(x => x.UserId == user_id && x.Default == true).FirstOrDefault();
                model.User = user;
                if (address != null)
                {
                    model.Address = address;
                    model.City = Convert.ToString(address.CityId);
                    model.District = Convert.ToString(address.DistrictId);
                    model.Ward = Convert.ToString(address.WardId);
                }
            }

            return View(model);
        }

        [HttpPost("thanh-toan")]
        public async Task<IActionResult> CheckBill(PageCheckBill model)
        {
            if (_cart.GetCount() < 0) return NotFound();

            var Email = "";
            int Tong = (int)_cart.GetTotal();

            if (!string.IsNullOrEmpty(model.Voucher))
            {

                var voucher = _context.VoucherModel.Where(x => x.Code.Equals(model.Voucher) && x.Active == true).FirstOrDefault();

                if (voucher != null)
                {
                    bool condition = true;

                    if (voucher.Condition == 1)
                    {
                        if (_cart.GetTotal() < voucher.NumberCondition)
                        {
                            // Tong = (int)_cart.GetTotal();
                        }
                        else
                        {
                            if (voucher.Voucher == 1)
                            {
                                Tong = Helper.CalculateTotal((int)_cart.GetTotal(), voucher.NumberVoucher);
                            }
                            else if (voucher.Voucher == 2)
                            {
                                Tong = Helper.CalculatePhanTram((int)_cart.GetTotal(), voucher.NumberVoucher);
                            }
                        }
                    }
                    else if (voucher.Condition == 2)
                    {
                        if (_cart.GetCount() < voucher.NumberCondition)
                        {
                            // Tong = (int)_cart.GetTotal();
                        }
                        else
                        {
                            if (voucher.Voucher == 1)
                            {
                                Tong = Helper.CalculateTotal((int)_cart.GetTotal(), voucher.NumberVoucher);
                            }
                            else if (voucher.Voucher == 2)
                            {
                                Tong = Helper.CalculatePhanTram((int)_cart.GetTotal(), voucher.NumberVoucher);
                            }
                        }
                    }

                }

            }

            var order = new OrderModel
            {
                Username = model.Address.Name,
                Address = model.Address.Address,
                CityId = int.Parse(model.City),
                DistrictId = int.Parse(model.District),
                WardId = int.Parse(model.Ward),
                Payment = model.Payment,
                State = 1,
                Phone = model.Address.Phone,
                OrderDate = DateTime.Now,
                Thanhtoan = false,
                Total = Tong
            };

            if (User.Identity.IsAuthenticated)
            {
                int user_id = int.Parse(User.Identity.Name ?? "");
                var user = _context.UserModel.Where(x => x.Id == user_id).FirstOrDefault();
                Email = user.Email;
                order.UserId = user_id;
            }
            else
            {
                Email = model.User.Email;
            }

            order.Email = Email;


            _context.OrderModel.Add(order);

            await _context.SaveChangesAsync();

            var order_id = order.OrderId;

            var carts = _cart.GetCartItems();

            foreach (var c in carts)
            {

                int Price = int.Parse(Helper.IfOrElse(c.Product.Discount ?? 0, c.Product.Price));

                _context.DetailOrderModel.Add(new DetailOrderModel
                {
                    OrderId = order_id,
                    ProductId = c.Product.Id,
                    Quantity = c.Count,
                    Price = Price
                });
            }

            _cart.EmptyCart();

            await _context.SaveChangesAsync();

            // return NotFound($"{order.Total} - {order.CityId} - {model.Voucher} - {Tong} - {order.OrderId}");

            return RedirectToAction("Orders");

            // var carts = _cart.GetCartItems();

            // var Carts = new List<ItemCartCheckBill>();

            // foreach (var item in carts)
            // {
            //     var Total = item.Product.Price * item.Count;
            //     if (item.Product.Discount > 0) Total = (item.Product.Discount ?? 0) * item.Count;

            //     Carts.Add(new ItemCartCheckBill
            //     {
            //         Cart = item,
            //         Total = Total
            //     });
            // }

            // model.Carts = Carts;

            // model.Cities = _context.CityModel.ToList();
            // model.Districts = _context.DistrictModel.ToList();
            // model.Wards = _context.WardModel.ToList();
            // model.Total = (int)_cart.GetTotal();

            // if (User.Identity.IsAuthenticated)
            // {
            //     int user_id = int.Parse(User.Identity.Name ?? "");
            //     var user = _context.UserModel.Where(x => x.Id == user_id).FirstOrDefault();
            //     model.User = user;
            // }


            // return View(model);
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("gio-hang")]
        public IActionResult Cart()
        {

            var model = new PageCartViewModel
            {
                Carts = _cart.GetCartItems(),
                Totail = (int)_cart.GetTotal()
            };

            return View(model);
        }

        public async Task<IActionResult> AddToCartDetail()
        {

            var ProId = int.Parse(Request.Form["pro_id"]);
            var Quantity = int.Parse(Request.Form["quantity"]);

            var Product = _context.ProductModel.Where(x => x.Id == ProId).FirstOrDefault();

            if (Product == null) return Json("");

            _cart.AddToCartDetail(Product, Quantity);

            return Json("Thêm sản phẩm thành công");

        }

        public async Task<IActionResult> UpdateCartItem()
        {

            var ProId = int.Parse(Request.Form["pro_id"]);
            var Quantity = int.Parse(Request.Form["quantity"]);

            var Product = _context.ProductModel.Where(x => x.Id == ProId).FirstOrDefault();

            if (Product == null) return Json("");

            var responres = _cart.UpdateCartItem(Product, Quantity);

            return Json(responres);

        }

        public async Task<IActionResult> GetDistrict()
        {

            var Id = int.Parse(Request.Form["id"]);

            var districts = _context.DistrictModel.Where(x => x.CityId == Id).ToList();

            if (districts == null) return Json("");

            return Json(districts);

        }

        public async Task<IActionResult> GetWard()
        {


            var Id = int.Parse(Request.Form["id"]);

            var wards = _context.WardModel.Where(x => x.DistrictID == Id).ToList();

            if (wards == null) return Json("");

            return Json(wards);

        }

        public async Task<IActionResult> ApplyVoucher()
        {

            var code = Request.Form["code"];

            var voucher = _context.VoucherModel.Where(x => x.Code.Equals(code) && x.Active == true).FirstOrDefault();

            if (voucher != null)
            {

                bool condition = true;

                if (voucher.Condition == 1)
                {
                    if (_cart.GetTotal() < voucher.NumberCondition)
                    {
                        return Json("Không đủ điều kiện sử dụng mã");
                    }
                    else
                    {
                        if (voucher.Voucher == 1)
                        {
                            return Json(Helper.CalculateTotal((int)_cart.GetTotal(), voucher.NumberVoucher));
                        }
                        else if (voucher.Voucher == 2)
                        {
                            return Json(Helper.CalculatePhanTram((int)_cart.GetTotal(), voucher.NumberVoucher));
                        }
                    }
                }
                else if (voucher.Condition == 2)
                {
                    if (_cart.GetCount() < voucher.NumberCondition)
                    {
                        return Json("Không đủ điều kiện sử dụng mã");
                    }
                    else
                    {
                        if (voucher.Voucher == 1)
                        {
                            return Json(Helper.CalculateTotal((int)_cart.GetTotal(), voucher.NumberVoucher));
                        }
                        else if (voucher.Voucher == 2)
                        {
                            return Json(Helper.CalculatePhanTram((int)_cart.GetTotal(), voucher.NumberVoucher));
                        }
                    }
                }

                return Json("Mã không hợp lên");
            }
            else
            {
                return Json("Mã này không tồn tại");
            }

        }


        public async Task<IActionResult> CategoryProductFilter()
        {

            // var strValue = Request.Form["str_value"];
            var arrValue = Request.Form["arr_value"];
            var strSlug = Convert.ToString(Request.Form["str_slug"]);

            var Values = JsonConvert.DeserializeObject<string[]>(arrValue);

            // Console.WriteLine($" --- {strSlug}");

            if (strSlug == "NULL")
            {

                if (Values.Count() <= 0) return Json(_context.ProductModel.Where(x => x.Active == true).ToList());
            }
            else
            {
                if (Values.Count() <= 0) return Json(_context.ProductModel.Where(x => x.Category.Slug == strSlug && x.Active == true).ToList());

            }


            var products = new List<ProductModel>();
            var response = new List<ProductModel>();

            string str = "";
            char s = 'n';

            // if(strSlug.Equals("NULL")){
            if (strSlug == "NULL")
            {
                // Console.WriteLine(12312313123123);
                foreach (var v in Values)
                {

                    // str = v.Split("AND")[0];
                    // s = str[0];

                    if (v.Split("AND").Count() > 1)
                    {
                        // Console.WriteLine($"{v.Split("AND")[0].Trim().Substring(1)} đến {v.Split("AND")[1].Trim().Substring(1)}");
                        int start = int.Parse(v.Split("AND")[0].Trim().Substring(1));
                        int end = int.Parse(v.Split("AND")[1].Trim().Substring(1));
                        // Console.WriteLine($"{start} đến {end}");
                        products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) >= start && (x.Discount > 0 ? x.Discount : x.Price) <= end && x.Active == true).ToList());
                    }
                    else
                    {
                        char calculator = v.Split("AND")[0][0];
                        int price = int.Parse(v.Split("AND")[0].Substring(1));
                        // Console.WriteLine($"{price} từ {calculator}");
                        if (calculator.Equals('<'))
                        {
                            // Console.WriteLine($"dấu {calculator}");
                            products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) < price && x.Active == true).ToList());
                            // products.AddRange(_context.ProductModel.Where(x => x.Price <= price && x.Active == true).ToList());
                        }
                        else if (calculator.Equals('>'))
                        {
                            // Console.WriteLine($"dấu {calculator}");
                            products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) > price && x.Active == true).ToList());
                        }
                    }

                }



            }
            else
            {

                var category = _context.CategoryModel.Where(x => x.Slug == strSlug && x.Active == true).FirstOrDefault();

                // Console.WriteLine($"--- {category.Id}");

                foreach (var v in Values)
                {

                    // str = v.Split("AND")[0];
                    // s = str[0];

                    if (v.Split("AND").Count() > 1)
                    {
                        // Console.WriteLine($"{v.Split("AND")[0].Trim().Substring(1)} đến {v.Split("AND")[1].Trim().Substring(1)}");
                        int start = int.Parse(v.Split("AND")[0].Trim().Substring(1));
                        int end = int.Parse(v.Split("AND")[1].Trim().Substring(1));
                        // Console.WriteLine($"{start} đến {end}");
                        products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) >= start && (x.Discount > 0 ? x.Discount : x.Price) <= end && x.CategoryId == category.Id && x.Active == true).ToList());
                    }
                    else
                    {
                        char calculator = v.Split("AND")[0][0];
                        int price = int.Parse(v.Split("AND")[0].Substring(1));
                        // Console.WriteLine($"{price} từ {calculator}");
                        if (calculator.Equals('<'))
                        {
                            // Console.WriteLine($"dấu {calculator}");
                            products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) < price && x.CategoryId == category.Id && x.Active == true).ToList());
                            // products.AddRange(_context.ProductModel.Where(x => x.Price <= price && x.Active == true).ToList());
                        }
                        else if (calculator.Equals('>'))
                        {
                            // Console.WriteLine($"dấu {calculator}");
                            products.AddRange(_context.ProductModel.Where(x => (x.Discount > 0 ? x.Discount : x.Price) > price && x.CategoryId == category.Id && x.Active == true).ToList());
                        }
                    }

                }



            }


            foreach (var item in products)
            {
                if (!response.Contains(item))
                {
                    response.Add(item);
                }
            }

            return Json(response);

        }


        [HttpPost("binh-luan")]
        public async Task<IActionResult> Comment(BoxCommentViewModel model)
        {
            // int UserId = int.Parse(HttpContext.Session.GetString("UserId"));

            var Product = _context.ProductModel.Where(x => x.Id == model.ProductId).FirstOrDefault();
            if (Product == null) return NotFound();

            int UserId = int.Parse(User.Identity.Name);
            if (UserId == 0)
            {
                ViewBag.Success = "Vui lòng đăng nhập để bình luận";
                return RedirectToAction("Detail", new { slug = Product.Slug });
            };

            // return NotFound($"123123sadsâdsasd -- {model.ProductId} -- {model.Comment}");

            var commentModel = new CommentModel()
            {
                UserId = UserId,
                ProductId = model.ProductId,
                Comment = model.Comment,
                Created = DateTime.Now
            };

            if (model.CommentParent != null)
            {
                commentModel.ParentId = model.CommentParent.Id;
            }

            _context.Add(commentModel);

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { slug = Product.Slug });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommentChild()
        {

            if (!User.Identity.IsAuthenticated) return NotFound();

            var user_id = int.Parse(User.Identity.Name);

            var pro_slug = Request.Form["pro_slug"];
            var reply = Request.Form["reply"];
            int pro_id = int.Parse(Request.Form["pro_id"]);
            int parent_id = int.Parse(Request.Form["parent_id"]);

            // return NotFound($"{pro_slug} - {pro_id} - {reply}");

            _context.CommentModel.Add(new CommentModel
            {
                UserId = user_id,
                ProductId = pro_id,
                Comment = reply,
                Created = DateTime.Now,
                ParentId = parent_id
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Home", new
            {
                slug = pro_slug
            });

        }


        public async Task<IActionResult> KeyWordProduct()
        {

            var Keyword = Request.Form["keyword"];

            var response = _context.ProductModel.Where(x => x.Keywords.Contains(Keyword) == true && x.Active == true).ToList();

            return Json(response);

        }




    }





}
