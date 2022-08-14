using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Catalog.PostImages;
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

        public Task<string> UploadImage(PostImageCreateRequest request);

        public Task<ApiResult<PageResult<PostImageVm>>> GetAllImage(GetPostImageRequest request);

        //   Task<int> AddImage(int PostId, PostImageCreateRequest request);
        //  Task<int> RemoveImage(int imageId);
        //  Task<int> UpdateImage(int imageId, PostImageUpdateRequest request);
        // Task<List<PostImageViewModel>> GetListImage(int PostId);
        //  Task<PostImageViewModel> GetImageById(int imageId)
        public Task<List<PostVm>> GetAll(string languageId);
    }
}
