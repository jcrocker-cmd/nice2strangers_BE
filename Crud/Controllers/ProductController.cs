using Microsoft.AspNetCore.Mvc;
using Crud.ViewModel;
using Crud.Models.Entities;
using Crud.Data;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        public ProductController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductViewModel productViewModel)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productViewModel.Image.CopyToAsync(stream);
            }

            var product = new Product
            {
                ProductName = productViewModel.ProductName,
                PriceInCents = productViewModel.PriceInCents,
                Image = $"/uploads/{fileName}",
                isActive = true
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return Ok(product);
        }

    }
}
