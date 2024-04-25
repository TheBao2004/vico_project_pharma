using Microsoft.AspNetCore.Mvc;
using Pharma.Data;
using Pharma.Models;
using Pharma.Services;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	public class CategoryController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;
		public Helper _helper;
		public IHttpContextAccessor _accessor;

		public CategoryController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
		{
			_context = context;
			_env = env;
			_accessor = accessor;
			_helper = new Helper(accessor, context, env);
		}

		public async Task<IActionResult> Create()
		{

			var model = new AdminCategoryViewModel
			{
				Categories = _context.CategoryModel.ToList(),
			};

			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Create(AdminCategoryViewModel model)
		{
			bool errors = true;

			if (_context.CategoryModel.Where(x => x.Slug == model.Category.Slug).FirstOrDefault() != null)
			{
				ModelState.AddModelError("Category.Slug", "Slug đã tồn tại");
				errors = false;
			}

			var file = Request.Form.Files["Anh"];

			if (file == null)
			{
				ModelState.AddModelError(".Category.Image", "Vui lòng chọn ảnh");
				errors = false;
			}


			if (errors)
			{

				string img = await _helper.Upload(file);
				model.Category.Image = img;

				var category = model.Category;

				_context.CategoryModel.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			model.Categories = _context.CategoryModel.ToList();
			return View(model);

		}


		public async Task<IActionResult> Index()
		{

			var categories = _context.CategoryModel.OrderByDescending(x => x.Id).ToList();

			var model = new List<AdminListCategoryViewModel>();

            foreach (var item in categories)
            {
				model.Add(new AdminListCategoryViewModel
				{
					Category = item,
					Categories = _context.CategoryModel.Where(x => x.CategoryId == item.Id).ToList(),
				});
            }

            return View(model);

		}

		public async Task<IActionResult> Edit(int id)
		{

			if (id == null) return NotFound();

			var category = _context.CategoryModel.Where(x => x.Id == id).FirstOrDefault();

			if (category == null) return NotFound();

			var model = new AdminCategoryViewModel
			{
               Category = category,
			   Categories = _context.CategoryModel.Where(x => x.Id != category.Id).ToList()
            };

            return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AdminCategoryViewModel model)
		{

			bool error = true;

			if (_context.CategoryModel.Where(x => x.Slug == model.Category.Slug && x.Id != model.Category.Id).FirstOrDefault() != null)
			{
				ModelState.AddModelError("Category.Slug", "Slug đã được sử dụng");
				error = false;
			}

			var file = Request.Form.Files["Anh"];

			if (file != null)
			{
				model.Category.Image = await _helper.Upload(file);
			}

			if (error)
			{

				_context.Update(model.Category);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");	

			}

			model.Categories = _context.CategoryModel.Where(x => x.Id != model.Category.Id).ToList();

			return View(model);

		}

	}
}
