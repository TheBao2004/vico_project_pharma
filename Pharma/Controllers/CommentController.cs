using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.Models;
using Pharma.Services;
using Pharma.ViewModels;

namespace Pharma.Controllers
{

    public class CommentController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public Helper _helper;
        public IHttpContextAccessor _accessor;

        public CommentController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;
            _helper = new Helper(accessor, context, env);
        }



        public async Task<IActionResult> Index(){

            var model = _context.CommentModel.Include(x => x.Product).Include(x => x.User).Where(x => x.ParentId == 0).ToList();

            return View(model);

        }

        public async Task<IActionResult> CommentChild(int id){

            var model = _context.CommentModel.Include(x => x.User).Include(x => x.Product).Where(x => x.ParentId == id).ToList();

            return View(model);

        }

        public async Task<IActionResult> Active(int id){

            var comment = _context.CommentModel.Find(id);

            if(comment.Active){
                comment.Active = false;
            }else{
                comment.Active = true;
            }

            _context.SaveChanges();

            if(comment.ParentId != 0) return RedirectToAction("CommentChild", new {id = comment.ParentId});

            return RedirectToAction("Index");

        }



    }


}