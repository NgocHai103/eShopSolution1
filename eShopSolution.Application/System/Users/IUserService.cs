using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
         Task<ApiResult<string>> Authencate(LoginRequest request);
         Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(Guid Id, UserUpdateRequest request);
      
        Task<ApiResult<PageResult<UserVM>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<UserVM>> GetById(Guid Id);
        Task<ApiResult<bool>> Delete(Guid Id);
        Task<ApiResult<bool>> RoleAssign(Guid Id,RoleAssignRequest request);

    }
}
