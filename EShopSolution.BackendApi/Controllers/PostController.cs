using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Posts;
using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Catalog.PostImages;
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
        [HttpGet("{postId}/{languageId}")]
        public async Task<IActionResult> GetById(int postId, string languageId)
        {
            var post = await _postSevice.GetById(postId, languageId);
            if (post == null)
                return BadRequest("Cannot find product");
            return Ok(post);
        }

        //localhost:port/api/post/images?pageIndex=1$pageSize=10&CategoryId=xx 
        [HttpGet("image")]
        public async Task<IActionResult> GetPaging([FromQuery] GetPostImageRequest request)
        {
            var products = await _postSevice.GetAllImage(request);
            return Ok(products);
        }

        [HttpPost("uploadImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] PostImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _postSevice.UploadImage(request);
            return Ok(new JsonResult(new { path = result }));
        }
        //[HttpGet("/getImage")]
        //public async Task<IActionResult> GetTokenImage()
        //{

        //    var token = new JwtSecurityToken("Tokens:Issuer",
        //        "Tokens:Issuer",
        //        expires: DateTime.Now.AddHours(3));
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        //}
    }
}
