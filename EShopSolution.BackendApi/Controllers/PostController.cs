using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Posts;
using eShopSolution.ViewModels.Catalog.Post;
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
            var products = await _postSevice.GetAllPaging(request);
            return Ok(products);
        }
    }
}
