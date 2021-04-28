using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{ 
    public interface IPostApiClient
    {
        public Task<ApiResult<PageResult<PostVm>>> GetPaging(GetPostPagingRequest request);
        public Task<ApiResult<bool>> Create(PostCreateRequest request);
        public Task<ApiResult<bool>> Update(PostUpdateRequest request);
        Task<PostVm> GetById(int id, string languageId);
        Task<bool> Delete(int id);
    }
}
