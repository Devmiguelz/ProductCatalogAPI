namespace ProductCatalogAPI.Application.DTO.ProductImage
{
    public class ProductImageUpdateDto
    {
        public Guid? Id { get; set; }
        public IFormFile? File { get; set; }
    }
}
