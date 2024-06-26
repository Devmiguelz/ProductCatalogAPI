using ProductCatalogAPI.Application.DTO.ProductImage;

namespace ProductCatalogAPI.Application.DTO.Product
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<ProductImageListDto> Images { get; set; } = null!;
    }
}
