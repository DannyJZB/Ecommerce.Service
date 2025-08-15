using Ecommerce.Api.Dtos;
using Ecommerce.Api.Interfaces;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Interfaces;

namespace Ecommerce.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobService;
        private readonly IImageService _imageService;
        

        public ProductService(IProductRepository productRepository, IBlobStorageService blobService, IImageService imageService)
        {
            _productRepository = productRepository;
            _blobService = blobService;
            _imageService = imageService;
        }

        public async Task<ProductDto> AddProduct(ProductDto productDto)
        {
            Products product = new Products()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Color = productDto.Color,
                Price = productDto.Price
            };

            _productRepository.AddProduct(product);

            using var stream = productDto.file?.OpenReadStream();

            string? extension = Path.GetExtension(productDto.file?.FileName)?.ToLowerInvariant();
            string? fileName = await _blobService.UploadAsync(stream, productDto.file?.FileName, productDto.file?.ContentType ?? "application/octet-stream", extension);

            _imageService.AddImage(fileName);

            Products? currentProduct = GetProduct(product.Name!);
            Images? currentImage = _imageService.GetImage(fileName);

            _productRepository.AddImageToProduct(currentProduct, currentImage);

            return productDto;
        }

        public Products? GetProduct(string productName)
        {
            return _productRepository.GetProduct(productName);
        }
    }
}
