﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Posts;
using eShopSolution.ViewModels.Catalog.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postSevice;
        public PostController(IPostService postSevice)
        {

            _postSevice = postSevice;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetPostPagingRequest request)
        {
            var Posts = await _postSevice.GetAllPaging(request);
            return Ok(Posts);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postSevice.Create(request);
            return Ok(result);
        }
        [HttpPut("{PostId}")]
        [Consumes("multipart/form-data")]

        //[Authorize]
        public async Task<IActionResult> Update([FromRoute] int PostId, [FromForm] PostUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postSevice.Update(PostId, request);
            return Ok(result);
        }
        [HttpDelete("{PostId}")]

       // [Authorize]
        public async Task<IActionResult> Delete(int PostId)
        {
            var affectedResult = await _postSevice.Delete(PostId);
            if (affectedResult == 0)
                return BadRequest();//return 400 error

            return Ok();
        }
    }
}
