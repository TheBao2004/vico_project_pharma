using System.Diagnostics;
using Pharma.Models;
using Pharma.Data;
using Pharma.Services;
using Pharma.ViewModels;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Pharma.Components
{

    public class MenuInforComponent : ViewComponent
    {


        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public ShoppingCart _cart { set; get; }

        private readonly ILogger<MenuInforComponent> _logger;

        public MenuInforComponent(ILogger<MenuInforComponent> logger, AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _accessor = accessor;
            _helper = new Helper(accessor, context, env);
            _cart = ShoppingCart.GetCart(context, accessor);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {   
            int user_id = int.Parse(User.Identity.Name);

            var model = new PageMenuInforViewModel{
                User = _context.UserModel.Where(x => x.Id == user_id).FirstOrDefault(),
                AddressCount = _context.AddressModel.Where(x => x.UserId == user_id).Count()
            };

            return View("~/Views/Shared/_MenuInfor.cshtml", model);

        }

    }

}