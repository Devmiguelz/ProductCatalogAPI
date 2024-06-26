namespace ProductCatalogAPI.Application.DTO.ProductImage
{
    public class ProductImageListDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
