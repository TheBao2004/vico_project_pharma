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

namespace Pharma.Components{

    public class HeaderComponent : ViewComponent{



	    private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;
		public Helper _helper;
		public IHttpContextAccessor _accessor;

		public HeaderComponent(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
		{
			_context = context;
			_env = env;
			_accessor = accessor;
			_helper = new Helper(accessor, context, env);
		}

        public async Task<IViewComponentResult> InvokeAsync(){

            List<ItemCategoryParent> CategoryModel = new List<ItemCategoryParent>();

            var Categories = _context.CategoryModel.Where(x => x.Active == true && x.CategoryId == 0).ToList();

            foreach (var cate in Categories)
            {
                CategoryModel.Add(new ItemCategoryParent{
                    Parent = cate,
                    Children = _context.CategoryModel.Where(x => x.Active == true && x.CategoryId == cate.Id).ToList()
                });
            }

            PageHeaderViewModel model = new PageHeaderViewModel () {
                Categories = CategoryModel
            };

            return View("~/Views/Shared/_Header.cshtml", model);

        }

    }

}