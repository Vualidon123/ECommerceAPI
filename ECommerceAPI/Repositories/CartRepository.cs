using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class CartRepository
    {
        private readonly ECommerceDbContext _context;
        public CartRepository(ECommerceDbContext context) {
            _context = context;
        }
        public async Task<Cart> GetCartByUserIdAsync(int userId) {
            return await _context.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.userId == userId);
        }
        public async Task<Cart> GetCartAsyncById(int id)
        {
            return await _context.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.id == id);
        }
        public async Task AddCartAsync(Cart cart) {
            // Enforce single cart per user and avoid duplicate key violation
            var existing = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.userId == cart.userId);
            if (existing != null) {
                throw new InvalidOperationException($"Cart already exists for userId={cart.userId} (cartId={existing.id}).");
            }
            cart.id = 0;
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartAsync(Cart cart) {
            var tracked = _context.ChangeTracker.Entries<Cart>()
                .FirstOrDefault(e => e.Entity.id == cart.id);
            if (tracked != null) {
                _context.Entry(tracked.Entity).State = EntityState.Detached;
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCartAsync(int cartId) {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null) {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}