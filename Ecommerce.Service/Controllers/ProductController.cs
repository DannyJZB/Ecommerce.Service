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

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto, CancellationToken ct)
        {
            if (productDto.file is null || productDto.file.Length == 0) return BadRequest("Archivo vacío.");
            return Ok(await _productService.AddProduct(productDto));
        }
    }
}