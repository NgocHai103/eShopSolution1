using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productSevice;
        public ProductsController(IProductService productSevice)
        {

            _productSevice = productSevice;
        }


        //localhost:port/api/product/paging?pageIndex=1$pageSize=10&CategoryId=xx 
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productSevice.GetAllPaging(request);
            return Ok(products);
        }
        [HttpGet("lastest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLastestProducts(int take, string languageId)
        {
            var products = await _productSevice.GetLatestProducts(languageId, take);
            return Ok(products);
        }
        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProducts(int take, string languageId)
        {
            var products = await _productSevice.GetFeaturedProducts(languageId, take);
            return Ok(products);
        }
        //localhost:port/api/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productSevice.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]

        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productSevice.Create(request);
            return Ok(result);
        }
        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]

        [Authorize]
        public async Task<IActionResult> Update([FromRoute]int productId,[FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productSevice.Update(productId,request);
            return Ok(result);
        }
        [HttpDelete("{productId}")]

        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productSevice.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();//return 400 error

            return Ok();
        }
        [HttpPatch("{productId}/{newPrice}")]

        [Authorize]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _productSevice.UpdatePrice(productId, newPrice);
            if (!isSuccessful)
                return BadRequest();//return 404 error
            return Ok();
        }

        //Images
        [HttpPost("{productId}/images")]

        [Authorize]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productSevice.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _productSevice.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productSevice.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productSevice.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productSevice.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
