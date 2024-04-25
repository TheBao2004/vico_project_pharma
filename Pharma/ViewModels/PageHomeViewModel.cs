

using Pharma.Models;

namespace Pharma.ViewModels{

    public class PageHomeViewModel{

        public List<ItemCategoryHome> Categories {set; get;}
        public List<ItemProduct> ProductBanner {set; get;}
        public List<ItemProduct> ProductSuaBot {set; get;}
        public List<ItemProduct> ProductSinhLy {set; get;}
        public List<VoucherModel> Vouchers {set; get;}
        public SectionProductForObject ProductForObject {set; get;}
        public SectionProductSuggest ProductSuggest {set; get;}
        public SectionProductDuocPham ProductDuocPham {set; get;}
        public List<BrandModel> Brands {set; get;}  


    }

    public class SectionProductDuocPham{

        public CategoryModel Category {set; get;}
        public List<KeywordModel> Keywords {set; get;}
        public List<ItemProduct> Products {set; get;}

    }

        public class SectionProductForObject{

        public CategoryModel Category {set; get;}
        public List<KeywordModel> Keywords {set; get;}
        public List<ItemProduct> Products {set; get;}

    }


     public class SectionProductSuggest{

        public CategoryModel Category {set; get;}
        public List<KeywordModel> Keywords {set; get;}
        public List<ItemProduct> Products {set; get;}

    }

    public class ItemCategoryHome{

        public CategoryModel Category {set; get;}
        public int NumberProduct {set; get;}

    } 


    public class ItemProduct{

        public ProductModel Product {set; get;}
        public string Thumbnail {set; get;}

    }


}