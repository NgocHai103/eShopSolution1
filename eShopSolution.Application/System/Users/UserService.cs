using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        private readonly RoleManager<AppRole> _roleManager;
      
        private readonly IConfiguration _config;
        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signManager, 
            RoleManager<AppRole> roleManager,
        IConfiguration config)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
         
            _config = config;
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {

            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user==null)
                throw new EShopException($"cannot find username :{request.UserName}");
            var result = await _signManager.PasswordSignInAsync(user,request.Password,request.RememberMe,true);
            if (!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
           // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<UserVM>> GetById(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                return new ApiErrorResult<UserVM>("User không tồn tại");
            var userVM = new UserVM()
            {

                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id
            };
            return new ApiSuccessResult<UserVM>(userVM);
        }

        public async Task<ApiResult<PageResult<UserVM>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if(!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(k=>k.UserName.Contains(request.Keyword)||k.PhoneNumber.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserVM()
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    Id = x.Id

                }).ToListAsync();
            var pagedResult = new PageResult<UserVM>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return new ApiSuccessResult<PageResult<UserVM>>(pagedResult);

        }



        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user!=null)
            {
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại.");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("Email đã đã được đăng kí.");
            }

            user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();
            else return new ApiErrorResult<bool>("Đăng kí không thành công.");
        }

        public async Task<ApiResult<bool>> Update(Guid Id,UserUpdateRequest request)
        {

            if (await _userManager.Users.AnyAsync(x=>x.Email==request.Email &&x.Id!=Id))
            {
                return new ApiErrorResult<bool>("Email đã đã được đăng kí.");
            }
            var user = await _userManager.FindByIdAsync(Id.ToString());
            user.Dob = request.Dob;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();
            else return new ApiErrorResult<bool>("Cập nhật không thành công.");
        }
    }
}
