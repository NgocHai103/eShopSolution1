using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class PostApiClient : BaseApiClient,IPostApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public PostApiClient(IHttpClientFactory httpClientFactory,
                 IHttpContextAccessor httpContextAccessor,
                  IConfiguration configuration)
          : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public Task<ApiResult<bool>> Create(PostCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PostVm> GetById(int id, string languageId)
        {
            var data = await GetAsync<PostVm>($"/api/post/{id}/{languageId}");
            return data;
        }

        public async Task<ApiResult<PageResult<PostVm>>> GetPaging(GetPostPagingRequest request)
        {
            var url = $"/api/post/paging?pageIndex=" +
             $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageid={request.LanguageId}";
            return await GetAsync<ApiResult<PageResult<PostVm>>>(url);
        }

        public Task<ApiResult<bool>> Update(PostUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
