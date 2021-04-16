using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryVm>> GetAll(string languageId);
        Task<CategoryVm> GetById(string languageId,int id);
        public Task<ApiResult<PageResult<CategoryVm>>> GetAllPaging(GetCategoryPagingRequest request);
        public Task<ApiResult<bool>> Create(CategoryCreateRequest request);
        public Task<ApiResult<bool>> Update(CategoryEditRequest request);
        public Task<int> Delete(int productId);
    }
}