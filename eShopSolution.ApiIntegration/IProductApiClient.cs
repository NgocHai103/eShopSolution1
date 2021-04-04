using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        public Task<ApiResult<PageResult<ProductVm>>> GetPaging(GetManageProductPagingRequest request);
        public Task<ApiResult<bool>> Create(ProductCreateRequest request);
        public Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ProductVm> GetById(int id, string languageId);
        Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take); 
        Task<List<ProductVm>> GetLastestProducts(string languageId, int take);
    }
}

