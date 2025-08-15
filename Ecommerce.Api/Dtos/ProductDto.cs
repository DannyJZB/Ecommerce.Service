using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Ecommerce.Api.Dtos
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; } = null;
        public long Price { get; set; }
        public string? Color { get; set; }
        [JsonIgnore]
        public IFormFile? file { get; set; }
    }
}
