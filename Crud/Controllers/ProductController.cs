using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Crud.Contracts;

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

    }
}
