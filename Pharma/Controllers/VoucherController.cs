using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.Models;
using Pharma.Services;
using Pharma.ViewModels;
namespace Pharma.Controllers
{

    public class VoucherController : Controller
    {


        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public VoucherController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;
            _helper = new Helper(accessor, context, env);
        }

        public async Task<IActionResult> Index(){

            return View(await _context.VoucherModel.ToListAsync());

        }

        public async Task<IActionResult> Create()
        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateVoucher model)
        {

            bool error = true;

            if (!_helper.IsInt(Convert.ToString(model.NumberCondition)))
            {
                ModelState.AddModelError("NumberCondition", "Dữ liệu phải là số");
                error = false;
            }

            if (_helper.IsInt(Convert.ToString(model.NumberVoucher)))
            {
                if (model.Voucher.Voucher == 2)
                {
                    if (int.Parse(model.NumberVoucher) < 1 || int.Parse(model.NumberVoucher) > 100)
                    {
                        ModelState.AddModelError("NumberVoucher", "Khi chọn phần trăm bạn chỉ được điền từ 1 đến 100");
                        error = false;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("NumberVoucher", "Dữ liệu phải là số");
                error = false;
            }

            if(error){

                model.Voucher.NumberCondition = int.Parse(model.NumberCondition);
                model.Voucher.NumberVoucher = int.Parse(model.NumberVoucher);

                _context.Add(model.Voucher);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id){

            if(id == null) return NotFound();

            var voucher = _context.VoucherModel.Where(x => x.Id == id).FirstOrDefault();

            if(voucher == null) return NotFound();

            var model = new AdminCreateVoucher{
                Voucher = voucher,
                NumberCondition = Convert.ToString(voucher.NumberCondition),
                NumberVoucher = Convert.ToString(voucher.NumberVoucher)
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminCreateVoucher model)
        {
            Console.WriteLine($"jojojojo{model.Voucher.Id}");
            bool error = true;

            if (!_helper.IsInt(Convert.ToString(model.NumberCondition)))
            {
                ModelState.AddModelError("NumberCondition", "Dữ liệu phải là số");
                error = false;
            }

            if (_helper.IsInt(Convert.ToString(model.NumberVoucher)))
            {
                if (model.Voucher.Voucher == 2)
                {
                    if (int.Parse(model.NumberVoucher) < 1 || int.Parse(model.NumberVoucher) > 100)
                    {
                        ModelState.AddModelError("NumberVoucher", "Khi chọn phần trăm bạn chỉ được điền từ 1 đến 100");
                        error = false;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("NumberVoucher", "Dữ liệu phải là số");
                error = false;
            }

            if(error){

                model.Voucher.NumberCondition = int.Parse(model.NumberCondition);
                model.Voucher.NumberVoucher = int.Parse(model.NumberVoucher);

                _context.VoucherModel.Update(model.Voucher);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id){

            if(id == null) return NotFound();

            var model = _context.VoucherModel.Where(x => x.Id == id).FirstOrDefault();

            if(model == null) return NotFound();

            _context.Remove(model);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

    }


}