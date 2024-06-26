using ProductCatalogAPI.Application.DTO.ProductImage;

namespace ProductCatalogAPI.Application.DTO.Product
{
    public class ProductUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductImageUpdateDto> Images { get; set; } = null!;
    }
}
