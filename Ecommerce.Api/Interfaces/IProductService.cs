using Ecommerce.Api.Dtos;
using Ecommerce.Data.Entities;

namespace Ecommerce.Api.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddProduct(ProductDto productDto);
        Products? GetProduct(string productName);
    }
}
