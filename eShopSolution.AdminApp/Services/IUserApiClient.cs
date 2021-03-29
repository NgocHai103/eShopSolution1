using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<ApiResult<String>> Authenticate(LoginRequest request);
        public Task<ApiResult<PageResult<UserVM>>> GetUserPaging(GetUserPagingRequest request);
        public Task<ApiResult<bool>> RegisterUser(RegisterRequest request);
        public Task<ApiResult<bool>> UpdaterUser(Guid Id,UserUpdateRequest request);
        public Task<ApiResult<UserVM>> GetById(Guid Id);
    }
}
