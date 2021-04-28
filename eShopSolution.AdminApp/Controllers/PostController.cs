using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostApiClient _postApiClient;
        private readonly IConfiguration _configuration;

        private readonly ICategoryApiClient _categoryApiClient;

        public PostController(IPostApiClient postApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _postApiClient = postApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            //var user = User.Identity.Name;
            var defaultLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetPostPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = defaultLanguageId
            };
            var data = await _postApiClient.GetPaging(request);
            ViewBag.Keyword = keyword;
            TempData["pageIndex"] = pageIndex;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
                return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var defaultLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var post = await _postApiClient.GetById(id, defaultLanguageId);
            var editVm = new PostUpdateRequest()
            {
                Id = post.Id,
                Description = post.Description,
                Content = post.Content,
                Name = post.Name,
                IsActive = post.IsActive,
                Images = post.Images
                //SeoDescription = post.SeoDescription,
                //SeoTitle = post.SeoTitle
            };
            return View(editVm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _postApiClient.Create(request);
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
