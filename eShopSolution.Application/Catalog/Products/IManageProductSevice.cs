
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products.Manage;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManagerProductSevice
    {
       public Task<int> Create(ProductCreateRequest request);
        public Task<int> Update(ProductUpdateRequest request);
        public Task<int> Delete(int productId);

        public Task<bool> UpdatePrice(int productId,decimal newPrice);
        public Task AddViewcount(int productId);
        public Task<bool> UpdateStock(int productId,int addedQuantity);
        public Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId,ProductImageUpdateRequest request);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
