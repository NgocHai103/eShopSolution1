using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id     {set;get;}
        public int Name   {set;get;}
        public int SortOrder{set;get;}
        public int IsShowOnHome{set;get;}
        public int? ParentId{set;get;}
        public Status Status {set;get;}
        public int SeoDescription{set;get;}
        public int SeoTitl { set; get; }

        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
