using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Posts
{
    public interface IPostService
    {
        public Task<ApiResult<bool>> Create(PostCreateRequest request);
        public Task<ApiResult<bool>> Update(int PostId, PostUpdateRequest request);
        public Task<PostVm> GetById(int PostId, string languageId);
        public Task<int> Delete(int PostId);
        public Task<ApiResult<PageResult<PostVm>>> GetAllPaging(GetPostPagingRequest request);

        public Task<string> UploadImage(IFormFile request);
        //   Task<int> AddImage(int PostId, PostImageCreateRequest request);
        //  Task<int> RemoveImage(int imageId);
        //  Task<int> UpdateImage(int imageId, PostImageUpdateRequest request);
        // Task<List<PostImageViewModel>> GetListImage(int PostId);
        //  Task<PostImageViewModel> GetImageById(int imageId)
        public Task<List<PostVm>> GetAll(string languageId);
    }
}
