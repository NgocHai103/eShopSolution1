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
            var result = await _userService.Authencate(request);
            if(result.IsSuccessed)
            {
                return BadRequest(result);
            }
            else
            {
              //  HttpContext.Session.SetString("Token",resultToken);
            }
            return Ok(result);
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
                return BadRequest(resutl);
            }
            return Ok(resutl);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resutl = await _userService.Update(id,request);
            if (!resutl.IsSuccessed)
            {
                return BadRequest(resutl);
            }
            return Ok(resutl);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resutl = await _userService.RoleAssign(id, request);
            if (!resutl.IsSuccessed)
            {
                return BadRequest(resutl);
            }
            return Ok(resutl);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetUserPagingRequest request)
        {
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var resutl = await _userService.Delete(id);
            return Ok(resutl);
        }
    }
}
