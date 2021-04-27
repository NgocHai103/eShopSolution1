using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Post
{
    public class PostVm
    {
        public int Id { set; get; }
        public DateTime DateCreated { set; get; }

        //tranlation
        public string Name { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }

        public List<string> Images { set; get; } = new List<string>();
        public string LanguageId { set; get; }
    }
}
