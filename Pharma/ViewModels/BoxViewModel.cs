

using System.ComponentModel.DataAnnotations;
using Pharma.Models;

namespace Pharma.ViewModels{

    public class BoxCommentViewModel{
		
        // public CommentModel Comment {set; get;}

		public int UserId { get; set; }

		public int ProductId { set; get; }

        public int ParentId {set; get;}

        [Display(Name = "Bình luận"), Required(ErrorMessage = "Vui lòng nhập")]
		public string Comment { get; set; } 

        public CommentModel CommentParent {set; get;}

    }

}