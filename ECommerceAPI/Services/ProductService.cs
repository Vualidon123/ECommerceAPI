using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }
        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
        }
        public async Task UpdateProductAsync(Product product)
        { 
            await _productRepository.UpdateProductAsync(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
        public async Task<List<Product>> SearchProduct(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<Product>();
            }

            // Run queries sequentially and combine results
            var byName = await _productRepository.GetProductByNameAsync(input);
            if (byName.Any())
            {
                return byName;
            }

            var byCategory = await _productRepository.GetProductsByCategoryAsync(input);
            return byCategory;
        }
    }
}
