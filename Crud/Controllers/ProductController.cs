using Crud.Contracts;
using Crud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
           _productService = productService;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductViewModel productViewModel)
        {
            var product = await _productService.AddProduct(productViewModel);
            return Ok(product);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _productService.GetAllAsync(); 
            return Ok(allProducts);
        }

        [HttpPut("updateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductViewModel productViewModel)
        {
            var updated = await _productService.UpdateProduct(id, productViewModel);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpGet("viewProduct/{id}")]
        public async Task<IActionResult> ViewProduct(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpPut("softDeleteProduct/{id}")]
        public async Task<IActionResult> SoftDeleteProduct(Guid id)
        {
            var product = await _productService.SoftDeleteProduct(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("recoverProduct/{id}")]
        public async Task<IActionResult> RecoverProduct(Guid id)
        {
            var product = await _productService.RecoverProduct(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("countActiveProducts")]
        public async Task<IActionResult> GetProductCounts()
        {
            var count = await _productService.GetProductCountsAsync();
            return Ok(count);
        }

        [HttpGet("totalStocks")]
        public async Task<IActionResult> GetTotalStocks()
        {
            var totalStocks = await _productService.GetTotalStocksAsync();
            return Ok(new { totalStocks });
        }

    }
}
