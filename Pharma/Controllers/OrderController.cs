using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.ViewModels;
using Pharma.Services;

namespace Pharma.Controllers{


    public class OrderController : Controller{


        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public OrderController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;   
            _helper = new Helper(accessor, context, env);
        }

        public async Task<IActionResult> Index(){

            var orders = _context.OrderModel.ToList();

            var model = new List<ItemOrderInfor>();

            foreach (var o in orders)
            {
                 model.Add(new ItemOrderInfor{
                    Order = o,
                    City = _context.CityModel.Where(x => x.Id == o.CityId).FirstOrDefault(),
                    District = _context.DistrictModel.Where(x => x.Id == o.DistrictId).FirstOrDefault(),
                    Ward = _context.WardModel.Where(x => x.ID == o.WardId).FirstOrDefault(),
                });
            }

            // return NotFound($" -- {model.Count()}");

            return View(model);

        }

        public async Task<IActionResult> Detail(int id){

          if(id == null) return NotFound();

            var order = _context.OrderModel.Where(x => x.OrderId == id).FirstOrDefault();

            if(order == null) return NotFound();

            var products = _context.DetailOrderModel.Include(x => x.Product).Where(x => x.OrderId == id).ToList();

            var model = new PageOrderDetailInfor(){
                Order = order,
                Products = products,
                City = _context.CityModel.Where(x => x.Id == order.CityId).FirstOrDefault(),
                District = _context.DistrictModel.Where(x => x.Id == order.DistrictId).FirstOrDefault(),
                Ward = _context.WardModel.Where(x => x.ID == order.WardId).FirstOrDefault(),
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Detail(PageOrderDetailInfor model){

            _context.OrderModel.Update(model.Order);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


    }


}