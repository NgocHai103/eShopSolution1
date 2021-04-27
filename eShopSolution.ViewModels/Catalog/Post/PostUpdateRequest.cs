using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Post
{
    public class PostUpdateRequest
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? IsActive { get; set; }
        public string LanguageId { set; get; }
    }
}
