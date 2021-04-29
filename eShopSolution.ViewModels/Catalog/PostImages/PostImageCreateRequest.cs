using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.PostImages
{
    public class PostImageCreateRequest
    {
        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
