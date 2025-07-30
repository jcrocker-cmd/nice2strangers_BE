using Crud.Models.Entities;
using Crud.Models;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<Product> AddProduct(ProductViewModel productViewModel);

    }
}
