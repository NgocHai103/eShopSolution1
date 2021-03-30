using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Sea.Services;

namespace Sea.Controllers
{
    public class UserController : Controller
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var sessions = context.HttpContext.Session.GetString("Token");
        //    if (sessions == null)
        //    {
        //        context.Result = new RedirectToActionResult("Index", "User", null);
        //    }
        //    base.OnActionExecuting(context);
        //}
        private readonly IUserServiceApp _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserServiceApp userService , IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public  IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _userService.Authenticate(request);

            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidationToken(result.ResultObj);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result.ResultObj);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
            if (result.IsSuccessed)
                return RedirectToAction("Index", "Home");
            ModelState.AddModelError("", result.Message);
            return View();

        }
        private ClaimsPrincipal ValidationToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];

            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];


            string key = _configuration["Tokens:Key"];

            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
