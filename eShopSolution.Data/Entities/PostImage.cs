using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class PostImage
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public DateTime DateCreated { get; set; }

        public long FileSize { get; set; }

        public Post Post { get; set; }
    }
}
