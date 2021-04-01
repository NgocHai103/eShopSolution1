using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public ProductController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
         //   _context = context;
        }
       
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            //var user = User.Identity.Name;
            var defaultLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
             var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = defaultLanguageId
            };
            var data = await _productApiClient.GetPaging(request);
             ViewBag.Keyword = keyword;
             if (TempData["result"] != null)
             {
                 ViewBag.SuccessMsg = TempData["result"];
             }
            return View(data.ResultObj);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
