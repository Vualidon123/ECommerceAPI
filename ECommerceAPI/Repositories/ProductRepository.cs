using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository
    { 
        private readonly ECommerceDbContext _context;
        public ProductRepository(ECommerceDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id) {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product) {
            _context.Products.Add(product);
            _context.SaveChanges(); ;
        }
        public async Task UpdateProductAsync(Product product) {
           

            _context.Products.Update(product);
           await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id) {
            var product = await _context.Products.FindAsync(id);
            if (product != null) {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
