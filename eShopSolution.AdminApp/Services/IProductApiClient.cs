using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IProductApiClient
    {
        public Task<ApiResult<PageResult<ProductVm>>> GetPaging(GetManageProductPagingRequest request);
        public Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ProductVm> GetById(int id, string languageId);
    }
}
