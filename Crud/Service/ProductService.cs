using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


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
                    Category = p.Category,
                    Description = p.Description,
                    Stocks = p.Stocks,
                    ProductName = p.ProductName,
                    PriceInCents = p.PriceInCents,
                    Image = p.Image,
                    isActive = p.isActive,
                    CreatedDate = p.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
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
                Stocks = (int) productViewModel.Stocks,
                PriceInCents = (int)(productViewModel.PriceInCents * 100),
                Image = $"/uploads/{fileName}",
                isActive = true,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProduct(Guid id, ProductViewModel productViewModel)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                return null;

            product.ProductName = productViewModel.ProductName;
            product.Category = productViewModel.Category;
            product.Description = productViewModel.Description;
            product.Stocks = (int)productViewModel.Stocks;
            product.PriceInCents = (int)(productViewModel.PriceInCents * 100);

            if (productViewModel.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productViewModel.Image.CopyToAsync(stream);
                }

                product.Image = $"/uploads/{fileName}";
            }

            product.UpdatedDate = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();

            return product;
        }


        public async Task<Product?> GetProductById(Guid id)
        {
            return await dbContext.Products.FindAsync(id);
        }

        public async Task<Product?> SoftDeleteProduct(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return null;

            product.isActive = false;
            product.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> RecoverProduct(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return null;

            product.isActive = true;
            product.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return product;
        }


        public async Task<ProductCountsViewModel> GetProductCountsAsync()
        {
            var activeCount = await dbContext.Products.CountAsync(p => p.isActive == true);
            var inactiveCount = await dbContext.Products.CountAsync(p => p.isActive == false);
            var totalCount = await dbContext.Products.CountAsync();

            return new ProductCountsViewModel
            {
                Active = activeCount,
                Inactive = inactiveCount,
                Total = totalCount
            };
        }

    }
}
