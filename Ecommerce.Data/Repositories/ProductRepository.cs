using Ecommerce.Data.Entities;
using Ecommerce.Data.Interfaces;

namespace Ecommerce.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IEcommerceDbContext _ecommerceDbContext;

        public ProductRepository(IEcommerceDbContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
        }

        public Products AddProduct(Products product)
        {
            _ecommerceDbContext.Products.Add(product);
            _ecommerceDbContext.SaveChanges();

            return product;
        }

        public Products? GetProduct(string productName)
        {
            return _ecommerceDbContext.Products.Where(x => x.Name!.Equals(productName)).OrderByDescending(c => c.ProductId).FirstOrDefault();
        }

        public bool AddImageToProduct(Products product, Images image)
        {
            _ecommerceDbContext.ProductImages.Add(new ProductImages() {  ProductId = product.ProductId, ImageId = image.ImageId });
            _ecommerceDbContext.SaveChanges();
            return true;
        }
    }
}
