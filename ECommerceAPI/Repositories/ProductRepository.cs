using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository
    {
        private readonly ECommerceDbContext _context;
        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            var trackedEntity = _context.ChangeTracker.Entries<Product>()
                .FirstOrDefault(e => e.Entity.id == product.id);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}