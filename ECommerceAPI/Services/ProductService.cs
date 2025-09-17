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
            var existingProduct = await _productRepository.GetProductByIdAsync(product.id);
            await _productRepository.UpdateProductAsync(existingProduct);
        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
