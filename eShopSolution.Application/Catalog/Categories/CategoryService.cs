using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Common;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.Utilities.Exceptions;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDBContext _context;

        public CategoryService(EShopDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(CategoryCreateRequest request)
        {
            var languages = _context.Languages;
            var tranlations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId
                    });

                }
                else
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = SystemConstants.ProductConstants.NA,
                        SeoDescription = SystemConstants.ProductConstants.NA,
                        SeoAlias = SystemConstants.ProductConstants.NA,
                        LanguageId = language.Id
                    });
                }
            }

            var category = new Category()
            {
                CategoryTranslations = tranlations,
            };
           
            _context.Categories.Add(category);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return new ApiErrorResult<bool>("Không thể thêm loại sản phẩm");
            return new ApiSuccessResult<bool>();
        }

        public async Task<int> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) throw new EShopException($"Cannot find a product: {categoryId}");
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryVm>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
                
            }).ToListAsync();
        }

        public async Task<ApiResult<PageResult<CategoryVm>>> GetAllPaging(GetCategoryPagingRequest request)
        {
            var query = from p in _context.Categories
                        join pt in _context.CategoryTranslations on p.Id equals pt.CategoryId into ppt
                        from pt in ppt.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name
                })
                .Distinct()
                .ToListAsync();
            var pageResult = new ApiSuccessResult<PageResult<CategoryVm>>(
                new PageResult<CategoryVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                });
            return pageResult;
        }

        public async Task<CategoryVm> GetById(string languageId, int id)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };
            return await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                IsShowOnHome = x.c.IsShowOnHome,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle,
                LanguageId = x.ct.LanguageId

            }).FirstOrDefaultAsync();
        }

        public async Task<ApiResult<bool>> Update(CategoryEditRequest request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
         //   var categorgyT = await _context.CategoryTranslations.FirstAsync(x => x.Id == request.Id && x.LanguageId == request.LanguageId);
            var categoryTralations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.Id && x.LanguageId == request.LanguageId);

            if (category == null || categoryTralations == null)
                return new ApiErrorResult<bool>("Sản phẩm ko thay");
            categoryTralations.Name = request.Name;
            categoryTralations.SeoAlias = request.SeoAlias;
            categoryTralations.SeoDescription = request.SeoDescription;
            categoryTralations.SeoTitle = request.SeoTitle;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return new ApiErrorResult<bool>("Sản phẩm chưa có thay đổi");
            return new ApiSuccessResult<bool>();
        }
    }
}