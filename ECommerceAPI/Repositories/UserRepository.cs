using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class UserRepository
    {
        private readonly ECommerceDbContext _context;
        public UserRepository(ECommerceDbContext context) { 
        _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync() {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id) {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.id == id);
        }
        public async Task<User> AddUserAsync(User user) {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUserAsync(User user) {
            var tracked = _context.ChangeTracker.Entries<User>()
                .FirstOrDefault(e => e.Entity.id == user.id);
            if (tracked != null) {
                _context.Entry(tracked.Entity).State = EntityState.Detached;
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<User?> Login(string email,string password)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.email.Equals(email)&&u.password.Equals(password));
            
        }
    }
}