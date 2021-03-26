using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductSevice;
        private readonly IManageProductService _manageProductSevice;
        public ProductController(IPublicProductService publicProductSevice, IManageProductService manageProductSevice)
        {
            _publicProductSevice = publicProductSevice;

            _manageProductSevice = manageProductSevice;
        }

        //localhost:port/api/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var products = await _publicProductSevice.GetAll(languageId);
            return Ok(products);
        }
        //localhost:port/api/product/public-paging
        [HttpGet("public-paging")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductSevice.GetAllByCategoryId(request);
            return Ok(products);
        }
        //localhost:port/api/product/1
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(int id,string languageId)
        {
            var product = await _manageProductSevice.GetById(id, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Creat([FromForm] ProductCreateRequest request)
        {
            var productId = await _manageProductSevice.Create(request);
            if (productId == 0)
                return BadRequest();//return 404 error

            var product = await _manageProductSevice.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] ProductUpdateRequest request)
        {
            var affectedResult = await _manageProductSevice.Update(request);
            if (affectedResult == 0)
                return BadRequest();//return 404 error

            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _manageProductSevice.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();//return 404 error

            return Ok();
        }
        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id,decimal newPrice)
        {
            var isSuccessful = await _manageProductSevice.UpdatePrice(id, newPrice);
            if (!isSuccessful)
                return BadRequest();//return 404 error
            return Ok();
        }

    }
}
