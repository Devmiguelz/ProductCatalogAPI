using AutoMapper;
using InventAutoApi.FileServer;
using ProductCatalogAPI.Application.DTO.Product;
using ProductCatalogAPI.Application.DTO.ProductImage;
using ProductCatalogAPI.Application.Exceptions;
using ProductCatalogAPI.Application.Interfaces;
using ProductCatalogAPI.Domain.Entities;
using ProductCatalogAPI.Domain.Interfaces;
using ProductCatalogAPI.Infrastructure.DAO;

namespace ProductCatalogAPI.Application 
{ 
    public class ProductService : IProductService 
    { 
        private readonly IProductDomain _productDomain; 
        private readonly IMapper _mapper;
        private readonly UploadImage _uploadImage;
        private readonly ApplicationDbContext _dbContext; 

        public ProductService(IProductDomain productDomain, UploadImage uploadImage, IMapper mapper, ApplicationDbContext dbContext) 
        { 
            _productDomain = productDomain; 
            _uploadImage = uploadImage;
            _mapper = mapper;
            _dbContext = dbContext; 
        }

        public async Task<ProductListDto?> GetProductByIdAsync(string id)
        {
            try
            {
                return _mapper.Map<ProductListDto>(await _productDomain.GetProductByIdAsync(Guid.Parse(id)));
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        public async Task<IEnumerable<ProductListDto>?> GetProductListAsync(List<Guid> guids)
        {
            try
            {
                return _mapper.Map<IEnumerable<ProductListDto>>(await _productDomain.GetProductListAsync(guids));
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        public async Task<IEnumerable<ProductListDto>?> GetProductsAsync()
        {
            try
            {
                return _mapper.Map<IEnumerable<ProductListDto>>(await _productDomain.GetProductsAsync());
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        public async Task<bool> SaveProductAsync(ProductCreateDto product)
        {
            try
            {
                var productSave = _mapper.Map<Product>(product);
                Guid vehicleId = await _productDomain.SaveProductAsync(productSave);
                await _dbContext.SaveChangesAsync();

                List<ProductImage> productImages = await AddProductImageAsync(vehicleId, product.Images);
                await _productDomain.SaveProductImageAsync(productImages);
                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            try
            {
                var productSaved = await _productDomain.GetProductByIdAsync(product.Id);
                if (productSaved is null)
                    return false;

                var productImageDelete = productSaved.ProductImage
                                                    .Where(x => !product.Images.Where(x => x.Id != null && x.Id != Guid.Empty).Any(i => i.Id.Equals(x.Id)))
                                                    .ToList();

                if (productImageDelete.Any())
                    await DeleteProductImageAsync(productImageDelete);

                var productImageSave = product.Images
                                                .Where(x => x.Id == null || x.Id == Guid.Empty)
                                                .Select(x => new ProductImageCreateDto
                                                {
                                                    File = x.File!
                                                })
                                                .ToList();
                
                List<ProductImage> vehicleImages = await AddProductImageAsync(productSaved.Id, productImageSave);
                var vehicleSave = _mapper.Map<Product>(product);

                bool updated = await _productDomain.UpdateProductAsync(vehicleSave);
                if (!updated)
                    return false;

                if (productImageSave.Any())
                    await _productDomain.SaveProductImageAsync(vehicleImages);

                return true;

            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        public async Task<bool> DeleteProductByIdAsync(string id)
        {
            try
            {
                var deleted = await _productDomain.DeleteProductByIdAsync(Guid.Parse(id));
                if (deleted)
                    await _dbContext.SaveChangesAsync();

                return deleted;
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorMessages.GenericError, ex);
            }
        }

        #region Private Method

        private async Task<List<ProductImage>> AddProductImageAsync(Guid productId, List<ProductImageCreateDto> productImageCreate)
        {
            List<ProductImage> vehicleImages = new();
            foreach (var image in productImageCreate.Select(x => x.File))
            {
                string urlImage = await _uploadImage.SaveImageAsync(image);

                var vehicleImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    FileName = image.FileName,
                    ImageURL = urlImage,
                    CreatedAt = DateTime.Now,
                };

                vehicleImages.Add(vehicleImage);
            }

            return vehicleImages;
        }

        private async Task DeleteProductImageAsync(ICollection<ProductImage> productImages)
        {
            foreach (var image in productImages)
            {
                _uploadImage.DeleteImage(image.ImageURL);
                await _productDomain.DeleteProductImageByIdAsync(image);
            }
        }

        #endregion
    }
} 
