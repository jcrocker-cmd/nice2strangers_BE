using Crud.Models.Entities;
using Crud.Models;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<Product> AddProduct(ProductViewModel productViewModel);
        Task<Product?> UpdateProduct(Guid id, ProductViewModel productViewModel);
        Task<Product?> GetProductById(Guid id);
        Task<Product?> SoftDeleteProduct(Guid id);
        Task<Product?> RecoverProduct(Guid id);
        Task<ProductCountsViewModel> GetProductCountsAsync();
    }
}
