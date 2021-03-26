using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductSevice;
        public ProductController(IPublicProductService publicProductSevice)
        {
            _publicProductSevice = publicProductSevice;

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _publicProductSevice.GetAll();
            return Ok(products);
        }
    }
}
