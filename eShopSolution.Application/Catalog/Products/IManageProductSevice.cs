using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manage;
using eShopSolution.Application.Dtos;
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
    }
}
