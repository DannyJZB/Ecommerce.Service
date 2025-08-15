using Ecommerce.Data.Entities;

namespace Ecommerce.Data.Interfaces
{
    public interface IProductRepository
    {
        Products? GetProduct(string productName);
        Products AddProduct(Products product);
        bool AddImageToProduct(Products product, Images image);
    }
}
