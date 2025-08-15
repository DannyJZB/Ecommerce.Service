namespace New.Ecommerce.Api.Interfaces
{
    public interface IProductService
    {
        Products AddProducts(Products product);
        List<Products> GetProducts();
    }
}
