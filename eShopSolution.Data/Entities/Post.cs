using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Post
    {
        public int Id { set; get; }
        public DateTime DateCreated { set; get; }
        public bool? IsActive { get; set; }
        public List<PostImage> PostImages { get; set; }
        public List<PostTranslation> PostTranslations { get; set; }
    }
}
