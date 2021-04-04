
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
       public Task<ApiResult<bool>> Create(ProductCreateRequest request);
        public Task<ApiResult<bool>> Update(int productId,ProductUpdateRequest request);
        public Task<ProductVm> GetById(int productId,string languageId);
        public Task<int> Delete(int productId);

        public Task<bool> UpdatePrice(int productId,decimal newPrice);
        public Task AddViewcount(int productId);
        public Task<bool> UpdateStock(int productId,int addedQuantity);
        public Task<ApiResult<PageResult<ProductVm>>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId,ProductImageUpdateRequest request);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<PageResult<ProductVm>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<List<ProductVm>> GetAll(string languageId);
        Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take);
        Task<List<ProductVm>> GetLatestProducts(string languageId, int take);
    }
}
