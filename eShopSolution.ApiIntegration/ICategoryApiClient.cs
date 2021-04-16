using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll(string languageId);
        Task<CategoryVm> GetById(string languageId,int id);
        public Task<ApiResult<PageResult<CategoryVm>>> GetPaging(GetCategoryPagingRequest request);
        public Task<ApiResult<bool>> Create(CategoryCreateRequest request);
        public Task<ApiResult<bool>> Update(CategoryEditRequest request);
        Task<bool> Delete(int id);
    }
}