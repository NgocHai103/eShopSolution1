using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resultToken = await _userService.Authencate(request);
            if(string.IsNullOrEmpty(resultToken.ResultObj))
            {
                return BadRequest("Username or passwork is incorrect.");
            }
            else
            {
              //  HttpContext.Session.SetString("Token",resultToken);
            }
            return Ok(resultToken);
        }
        [HttpPost]
        [AllowAnonymous] // don't need add Token
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resutl = await _userService.Register(request);
            if (!resutl.IsSuccessed)
            {
                return BadRequest("Register is unsuccessful.");
            }
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resutl = await _userService.Register(request);
            if (!resutl.IsSuccessed)
            {
                return BadRequest("Register is unsuccessful.");
            }
            return Ok();
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetUserPagingRequest request)
        {
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }
    }
}
