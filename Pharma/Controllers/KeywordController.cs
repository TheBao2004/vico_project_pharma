
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.Models;
using Pharma.Services;
using Pharma.ViewModels;

namespace Pharma.Controllers
{

    public class KeywordController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public KeywordController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;
            _helper = new Helper(accessor, context, env);
        }

        public async Task<IActionResult> Index()
        {

            var model = _context.KeywordModel.Include(x => x.Category).ToList();

            return View(model);

        }


        public async Task<IActionResult> Create()
        {

            var model = new PageActionKeywordViewModel
            {
                Categories = _context.CategoryModel.ToList()
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(PageActionKeywordViewModel model)
        {

            _context.KeywordModel.Add(model.Keyword);

            await _context.SaveChangesAsync();

            ViewBag.Success = "Thêm thành công";

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {

            if (id == null) return NotFound();

            var keyword = _context.KeywordModel.Find(id);

            if (keyword == null) return NotFound();

            var model = new PageActionKeywordViewModel
            {
                Categories = _context.CategoryModel.ToList(),
                Keyword = keyword
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(PageActionKeywordViewModel model){

            _context.KeywordModel.Update(model.Keyword);

            await _context.SaveChangesAsync();

            ViewBag.Success = "Sửa thành công";

            return RedirectToAction("Index");

        }

    }


}