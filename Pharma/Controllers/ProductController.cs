using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using PagedList;
using Pharma.Data;
using Pharma.Models;
using Pharma.Services;
using Pharma.ViewModels;

namespace Pharma.Controllers
{
	public class ProductController : Controller
	{

		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;
		public Helper _helper;
		public IHttpContextAccessor _accessor;

		public ProductController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
		{
			_context = context;
			_env = env;
			_accessor = accessor;
			_helper = new Helper(accessor, context, env);
		}


		public async Task<IActionResult> Index(int? page)
		{
			int pageSive = 5;
			ViewBag.pageSive = pageSive;

			if(page == null){
				page = 1;
			}

			// return View(_context.ProductModel.Include(x => x.Category).Include(x => x.Brand).OrderByDescending(x => x.Id).ToList().ToPagedList((int)page, pageSive));
			return View(_context.ProductModel.Include(x => x.Category).Include(x => x.Brand).OrderByDescending(x => x.Id).ToList());

		}

		public async Task<IActionResult> Create()
		{

			var model = new AdminCreateProductViewModel
			{
				Categories = _context.CategoryModel.ToList(),
				Brands = _context.BrandModel.ToList(),
				Keywords = _context.KeywordModel.ToList()
			};

			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Create(AdminCreateProductViewModel model)
		{

			string Keyword = "";

			if(model.arrKeyword != null){
				Keyword = string.Join(",", model.arrKeyword);
			}

			// foreach (var item in model.arrKeyword)
			// {
			// 	Console.WriteLine($" -- {item}");
			// }

			// return NotFound($" --- {model.arrKeyword[1]} - {model.Product.Name} - {Keyword}");

			bool error = true;

			if (!Regex.IsMatch(model.Price, @"^[0-9]+$"))
			{
				ModelState.AddModelError("Product.Price", "CHỉ được chuyền số dương");
				error = false;
			}

			if (!Regex.IsMatch(model.Quantity, @"^[0-9]+$"))
			{
				ModelState.AddModelError("Product.Quantity", "CHỉ được chuyền số dương");
				error = false;
			}

			if (!string.IsNullOrEmpty(model.Discount))
			{
				if (!Regex.IsMatch(model.Discount, @"^[0-9]+$"))
				{
					ModelState.AddModelError("Product.Discount", "CHỉ được chuyền số dương");
					error = false;
				}
			}

			if (_context.ProductModel.Where(x => x.Slug == model.Product.Slug).FirstOrDefault() != null)
			{
				ModelState.AddModelError("Product.Slug", "Slug này đã được dùng");
				error = false;
			}

			if (error)
			{

				model.Product.Price = int.Parse(model.Price);
				model.Product.Quantity = int.Parse(model.Quantity);
				model.Product.Discount = string.IsNullOrWhiteSpace(model.Discount) ? 0 : int.Parse(model.Discount);
				model.Product.Keywords = Keyword;

				_context.Add(model.Product);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index");

			}

			model.Categories = _context.CategoryModel.ToList();
			model.Brands = _context.BrandModel.ToList();

			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{

			if (id == null) return NotFound();

			var model = _context.ProductModel.Where(x => x.Id == id).FirstOrDefault();

			if (model == null) return NotFound();

			return View(new AdminCreateProductViewModel
			{
				Product = model,
				Categories = _context.CategoryModel.ToList(),
				Brands = _context.BrandModel.ToList(),
				Discount = Convert.ToString(model.Discount),
				Price = Convert.ToString(model.Price),
				Quantity = Convert.ToString(model.Quantity),
				Keywords = _context.KeywordModel.ToList()
			});
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AdminCreateProductViewModel model)
		{

			string Keyword = "";

			if(model.arrKeyword != null){
				Keyword = string.Join(",", model.arrKeyword);
			}

			bool error = true;

			if (!Regex.IsMatch(model.Price, @"^[0-9]+$"))
			{
				ModelState.AddModelError("Product.Price", "CHỉ được chuyền số dương");
				error = false;
			}

			if (!Regex.IsMatch(model.Quantity, @"^[0-9]+$"))
			{
				ModelState.AddModelError("Product.Quantity", "CHỉ được chuyền số dương");
				error = false;
			}

			if (!string.IsNullOrEmpty(model.Discount))
			{
				if (!Regex.IsMatch(model.Discount, @"^[0-9]+$"))
				{
					ModelState.AddModelError("Product.Discount", "CHỉ được chuyền số dương");
					error = false;
				}
			}

			if (_context.ProductModel.Where(x => x.Slug == model.Product.Slug && x.Id != model.Product.Id).FirstOrDefault() != null)
			{
				ModelState.AddModelError("Product.Slug", "Slug này đã được dùng");
				error = false;
			}

			if (error)
			{

				model.Product.Price = int.Parse(model.Price);
				model.Product.Quantity = int.Parse(model.Quantity);
				model.Product.Discount = string.IsNullOrWhiteSpace(model.Discount) ? 0 : int.Parse(model.Discount);
				model.Product.Keywords = Keyword;

				_context.Update(model.Product);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index");

			}

			model.Categories = _context.CategoryModel.ToList();
			model.Brands = _context.BrandModel.ToList();
			model.Keywords = _context.KeywordModel.ToList();

			return View(model);
		}


		public async Task<IActionResult> UploadProduct()
		{

			// var id = int.Parse(Request.Form["id"]);
			var img = Request.Form.Files;
			// Console.WriteLine($"hihihihihi{img.FileName}");	
			// var nameImg = await _helper.UploadMuch(img);
			var nameImg = await _helper.UploadMuch(img);

			Console.WriteLine(nameImg);

			// var product = _context.ProductModel.Where(x => x.Id == int.Parse(id)).FirstOrDefault();

			// if(product == null) return Json("");

			List<string> arrImage = new List<string>();

			// if(!string.IsNullOrEmpty(product.Images)){
			// 	arrImage.AddRange(JsonSerializer.Deserialize<string[]>(product.Images));
			// }

			arrImage.Add(nameImg);

			string json = JsonSerializer.Serialize(arrImage);

			// product.Images = json;

			// _context.Update(product);

			// await _context.SaveChangesAsync();

			return Json(nameImg);
		}


		public async Task<IActionResult> AddImageProduct()
		{

			var id = Request.Form["id"];
			var image = Request.Form["image"];

			var product = _context.ProductModel.Where(x => x.Id == int.Parse(id)).FirstOrDefault();

			if (product == null) return Json("");

			var strImg = product.Images  + image + ",";

			product.Images = strImg;

			_context.Update(product);

			await _context.SaveChangesAsync();

			return Json(image);

		}

		public IActionResult getAllImage(int id)
		{

			if (id == null) return Json("");

			var product = _context.ProductModel.Where(x => x.Id == id).FirstOrDefault();

			if (product == null) return Json("");

			return Json(product.Images);

		}

		public IActionResult RemoveImage()
		{

			var id = int.Parse(Request.Form["id"]);
			var image = Convert.ToString(Request.Form["image"]);

			Console.WriteLine("asdasdasd " + image);

			if (id == null) return Json("");

			var product = _context.ProductModel.Where(x => x.Id == id).FirstOrDefault();

			if (product == null) return Json("");

			var strImg = product.Images;

			var arrImg = strImg.Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray();

			var filterImg = arrImg.Where(x => x != image).ToArray();

			strImg = string.Join(",", filterImg);
			
			// var start = strImg.IndexOf(image);
			// int end = start + image.Length + 1;
			// int duoi = end;

			// var res = strImg.Remove(start, duoi);
			// Console.WriteLine($"12312312321312: {strImg}");
			// Console.WriteLine($"12312312321312: {start} {end} {image}");
			// Console.WriteLine($"12312312321312: {res}");

			product.Images = strImg;

			_context.Update(product);

			_context.SaveChanges();

			_helper.RemoveImage(image);

			return Json(strImg);

		}

	}
}
