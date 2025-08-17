using Ecommerce.Api.Dtos;
using Ecommerce.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBlobStorageService _blobStorageService;

        public ProductController(IProductService productService, IBlobStorageService blobStorageService)
        {
            _productService = productService;
            _blobStorageService = blobStorageService;
        }

        [HttpPost("Product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto, CancellationToken ct)
        {
            if (productDto.file is null || productDto.file.Length == 0) return BadRequest("Archivo vacío.");
            return Ok(await _productService.AddProduct(productDto));
        }

        [HttpGet("SAS")]
        public async Task<IActionResult> GenerateSAS(string fileName)
        {
            var sasUrl = await _blobStorageService.GenerateSASToken(fileName); // tu lógica
            return Ok(new { sasUrl });
        }
    }
}