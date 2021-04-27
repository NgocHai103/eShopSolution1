using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class PostTranslation
    {
        public int Id { set; get; }
        public int PostId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public string LanguageId { set; get; }

        public Post Post { get; set; }

        public Language Language { get; set; }
    }
}
