using ProductCatalogAPI.Application.DTO.Product;

namespace ProductCatalogAPI.Application.Interfaces 
{ 
    public interface IProductService 
    {    
        public Task<IEnumerable<ProductListDto>?> GetProductsAsync();
        public Task<ProductListDto?> GetProductByIdAsync(string id);
        public Task<IEnumerable<ProductListDto>?> GetProductListAsync(List<Guid> guids);
        public Task<bool> SaveProductAsync(ProductCreateDto product);
        public Task<bool> UpdateProductAsync(ProductUpdateDto product);
        public Task<bool> DeleteProductByIdAsync(string id);
    } 
}  
