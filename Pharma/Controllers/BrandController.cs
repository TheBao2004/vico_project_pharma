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

namespace Pharma.Controllers
{
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public BrandController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;   
            _helper = new Helper(accessor, context, env);
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
              return _context.BrandModel != null ? 
                          View(await _context.BrandModel.OrderBy(x => x.Id).ToListAsync()) :
                          Problem("Entity set 'AppDbContext.BrandModel'  is null.");
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BrandModel == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brandModel)
        {

            var file = Request.Form.Files["Image"];

            bool errors = true;

            if (file == null)
            {
                ModelState.AddModelError("Image", "Vui lòng chọn ảnh");
                errors = false;
            }

            if (_context.BrandModel.Where(x => x.Slug == brandModel.Slug).FirstOrDefault() != null)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại");
                errors = false;
            }

            if (errors)
            {
                string nameImg = await _helper.Upload(file);

                brandModel.Image = nameImg;

                _context.Add(brandModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(brandModel);
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BrandModel == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel.FindAsync(id);
            if (brandModel == null)
            {
                return NotFound();
            }
            return View(brandModel);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(BrandModel brandModel)
        {
            var file = Request.Form.Files["Anh"];

            bool errors = true;

            if (file != null)
            {
                string nameImg = await _helper.Upload(file, brandModel.Image);
                brandModel.Image = nameImg;
            }

            if (_context.BrandModel.Where(x => x.Slug == brandModel.Slug && x.Id != brandModel.Id).FirstOrDefault() != null)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại");
                errors = false;
            }

            if (errors)
            {

                _context.Update(brandModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(brandModel);
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BrandModel == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BrandModel == null)
            {
                return Problem("Entity set 'AppDbContext.BrandModel'  is null.");
            }
            var brandModel = await _context.BrandModel.FindAsync(id);
            if (brandModel != null)
            {
                _context.BrandModel.Remove(brandModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandModelExists(int id)
        {
          return (_context.BrandModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
