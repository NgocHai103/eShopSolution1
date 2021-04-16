using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //localhost:port/api/Category/paging?pageIndex=1$pageSize=10&keyword=xx 
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetCategoryPagingRequest request)
        {
            var products = await _categoryService.GetAllPaging(request);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var products = await _categoryService.GetAll(languageId);
            return Ok(products);
        }
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(string languageId,int id)
        {
            var products = await _categoryService.GetById(languageId,id);
            return Ok(products);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.Create(request);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update( [FromForm] CategoryEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.Update(request);
            return Ok(result);
        }

        [HttpDelete("{productId}")]

        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _categoryService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();//return 400 error

            return Ok();
        }
    }
}