using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Post
{
    public class PostCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập tên")]
        public string Name { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? IsActive { get; set; }
        public string LanguageId { set; get; }
    }
}
