using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.PostImages
{
    public class GetPostImageRequest: PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
