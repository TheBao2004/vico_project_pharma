
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.ViewModels;

namespace Pharma.Components{

    public class CommentComponent : ViewComponent{

        private readonly AppDbContext _context;

        public CommentComponent(AppDbContext context){
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProId, int Id){

            var model = new BoxCommentViewModel {
                ProductId = ProId
            };

            if(Id != 0){
                var Comment = _context.CommentModel.Include(x => x.User).Where(x => x.Id == Id).FirstOrDefault();
                model.CommentParent = Comment;    
            }

            return View("~/Views/Shared/_Comment.cshtml", model);

        }

    }

}