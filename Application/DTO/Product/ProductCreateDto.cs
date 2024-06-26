using Newtonsoft.Json;
using ProductCatalogAPI.Application.DTO.ProductImage;

namespace ProductCatalogAPI.Application.DTO.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [JsonRequired]
        public List<ProductImageCreateDto> Images { get; set; } = null!;
    }
}
