using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain.Entities;
using ProductCatalogAPI.Domain.Interfaces;
using ProductCatalogAPI.Infrastructure.DAO;

namespace ProductCatalogAPI.Domain.Services
{
    public class ProductDomain : IProductDomain
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductDomain(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _dbContext.Product.AsNoTracking().Include(x => x.ProductImage).Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<Product>> GetProductListAsync(List<Guid> guids)
        {
            return await _dbContext.Product.AsNoTracking().Include(x => x.ProductImage).Where(x => !x.IsDeleted && guids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dbContext.Product.AsNoTracking().Include(x => x.ProductImage).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Guid> SaveProductAsync(Product product)
        {
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.Now;
            product.IsDeleted = false;

            await _dbContext.Product.AddAsync(product);
            return product.Id;
        }

        public async Task<bool> SaveProductImageAsync(List<ProductImage> imageList)
        {
            await _dbContext.ProductImage.AddRangeAsync(imageList);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductImageByIdAsync(ProductImage productImage)
        {
            _dbContext.ProductImage.Remove(productImage);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductAsync(Product vehicle)
        {
            var ExistProduct = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id.Equals(vehicle.Id));
            if (ExistProduct == null)
                return false;

            vehicle.CreatedAt = ExistProduct.CreatedAt;
            vehicle.UpdatedAt = DateTime.Now;
            vehicle.IsDeleted = ExistProduct.IsDeleted;

            _dbContext.Entry(ExistProduct).CurrentValues.SetValues(vehicle);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            var ExistProduct = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (ExistProduct == null)
                return false;

            ExistProduct.DeletedAt = DateTime.Now;
            ExistProduct.IsDeleted = true;

            return true;
        }
    }
} 
