using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductSevice;
        private readonly IManageProductService _manageProductSevice;
        public ProductsController(IPublicProductService publicProductSevice, IManageProductService manageProductSevice)
        {
            _publicProductSevice = publicProductSevice;

            _manageProductSevice = manageProductSevice;
        }

        
        //localhost:port/api/product?pageIndex=1$pageSize=10&CategoryId=xx 
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetPaging(string languageId,[FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductSevice.GetAllByCategoryId(languageId,request);
            return Ok(products);
        }
        //localhost:port/api/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductSevice.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Creat([FromForm] ProductCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductSevice.Create(request);
            if (productId == 0)
                return BadRequest();//return 404 error

            var product = await _manageProductSevice.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductSevice.Update(request);
            if (affectedResult == 0)
                return BadRequest();//return 400 error

            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _manageProductSevice.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();//return 400 error

            return Ok();
        }
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _manageProductSevice.UpdatePrice(productId, newPrice);
            if (!isSuccessful)
                return BadRequest();//return 404 error
            return Ok();
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductSevice.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _manageProductSevice.GetImageById(imageId);

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
            var result = await _manageProductSevice.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _manageProductSevice.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }


    }
}
