using MillyBeads.Models.Entities;
using MillyBeads.Models.ViewModels;

namespace MillyBeads.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(ProductViewModel viewModel);
        Task<Product> UpdateProductAsync(int id, ProductViewModel viewModel);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
} 