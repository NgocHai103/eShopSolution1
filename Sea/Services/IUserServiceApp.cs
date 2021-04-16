using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sea.Services
{
    public interface IUserServiceApp
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
    }
}
