using ProductCatalogAPI.Domain.Entities;

namespace ProductCatalogAPI.Domain.Interfaces 
{ 
    public interface IProductDomain 
    {
        public Task<List<Product>> GetProductsAsync();
        public Task<List<Product>> GetProductListAsync(List<Guid> guids);
        public Task<Product?> GetProductByIdAsync(Guid id);
        public Task<Guid> SaveProductAsync(Product product);
        public Task<bool> SaveProductImageAsync(List<ProductImage> imageList);
        public Task<bool> DeleteProductImageByIdAsync(ProductImage productImage);
        public Task<bool> UpdateProductAsync(Product product);
        public Task<bool> DeleteProductByIdAsync(Guid id);
    } 
} 
