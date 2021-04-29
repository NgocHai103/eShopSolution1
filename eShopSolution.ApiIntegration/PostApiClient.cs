using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Catalog.PostImages;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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


        public async Task<ApiResult<string>> UploadImage(PostImageCreateRequest request)
        {
            var sessions = _httpContextAccessor
              .HttpContext
              .Session
              .GetString(SystemConstants.AppSettings.Token);
            var defaultLanguageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ImageFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ImageFile.OpenReadStream().Length);
                }
                var bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ImageFile", request.ImageFile.FileName);
            }

            requestContent.Add(new StringContent(request.IsDefault.ToString()), "IsDefault");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Caption) ? "" : request.Caption.ToString()), "Caption");
            requestContent.Add(new StringContent(defaultLanguageId), "languageId");

            var response = await client.PostAsync($"/api/post/uploadImage", requestContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ApiSuccessResult<string>(result);

            return new ApiErrorResult<string>(result);
        }
    }
}
