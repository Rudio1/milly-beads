using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MillyBeads.Models.Entities;
using MillyBeads.Models.ViewModels;
using MillyBeads.Models.Repositories;

namespace MillyBeads.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IProductRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath
            });
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(ProductViewModel viewModel)
        {
            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price
            };

            if (viewModel.ImageFile != null)
            {
                product.ImagePath = await SaveImageAsync(viewModel.ImageFile);
            }

            await _repository.AddAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductViewModel viewModel)
        {
            var product = await _repository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;

            if (viewModel.ImageFile != null)
            {
                // Deleta a imagem antiga se existir
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    DeleteImage(product.ImagePath);
                }

                // Upload da nova imagem
                product.ImagePath = await SaveImageAsync(viewModel.ImageFile);
            }

            await _repository.UpdateAsync(product);
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    DeleteImage(product.ImagePath);
                }

                await _repository.DeleteAsync(id);
            }
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product != null;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/products/" + uniqueFileName;
        }

        private void DeleteImage(string imagePath)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
} 