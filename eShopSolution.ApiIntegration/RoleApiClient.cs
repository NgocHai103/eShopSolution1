using eShopSolution.Application.System.Roles;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IRoleService _roleService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        IHttpContextAccessor _httpContextAccessor;
        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token")); //"Bearer" + 
            var response = await client.GetAsync($"/api/roles");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<RoleVm> roles = JsonConvert.DeserializeObject<List<RoleVm>>(body);
                return new ApiSuccessResult<List<RoleVm>>(roles);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVm>>>(body);
        }
    }
}
