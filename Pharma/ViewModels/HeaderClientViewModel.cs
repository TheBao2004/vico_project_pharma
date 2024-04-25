

using Pharma.Models;

namespace Pharma.ViewModels{

    public class PageHeaderViewModel{

        public List<ItemCategoryParent> Categories {set; get;}


    }


    public class ItemCategoryParent{

        public CategoryModel Parent {set; get;}

        public List<CategoryModel> Children {set; get;}

    }
 

}