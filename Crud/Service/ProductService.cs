using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Crud.Contracts;


namespace Crud.Service
{
    public class ProductService : IProductService
    {
        public readonly ApplicationDBContext dbContext;

        public ProductService(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await dbContext.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    PriceInCents = p.PriceInCents,
                    Image = p.Image,
                    isActive = p.isActive,
                    CreatedDate = p.CreatedDate.ToString()

                })
                .ToListAsync();
        }

        public async Task<Product> AddProduct(ProductViewModel productViewModel)
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
                Category = productViewModel.Category,
                Description = productViewModel.Description,
                Stocks = productViewModel.Stocks,
                PriceInCents = (int)(productViewModel.PriceInCents * 100),
                Image = $"/uploads/{fileName}",
                isActive = true,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return product;
        }
    }
}
