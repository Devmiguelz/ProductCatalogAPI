using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Application.DTO.Product;
using ProductCatalogAPI.Application.Interfaces;

namespace ProductCatalogAPI.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class ProductController : ControllerBase 
    { 
        private readonly IProductService _productService; 

        public ProductController(IProductService productServic) 
        { 
            _productService = productServic; 
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetProducts()
        {
            var product = await _productService.GetProductsAsync();
            if (product is null)
                return NoContent();

            return Ok(product);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ProductListDto>> GetProductByIdAsync(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
                return NoContent();

            return Ok(product);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProductListDto>> GetProductListAsync([FromBody] List<Guid> guids)
        {
            var product = await _productService.GetProductListAsync(guids);
            if (product is null)
                return NoContent();

            return Ok(product);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> SaveProductAsync([FromForm] ProductCreateDto product)
        {
            var productSaved = await _productService.SaveProductAsync(product);
            if (!productSaved)
                return BadRequest();

            return Ok(productSaved);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<bool>> UpdateProductAsync([FromForm] ProductUpdateDto product)
        {
            var productUpdated = await _productService.UpdateProductAsync(product);
            if (!productUpdated)
                return BadRequest();

            return Ok(productUpdated);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteProductById(string id)
        {
            bool delete = await _productService.DeleteProductByIdAsync(id);
            if (!delete)
                return NoContent();

            return Ok(delete);
        }
    } 
} 
