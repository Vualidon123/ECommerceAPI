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
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> AddUserAsync(User user) {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        public async Task UpdateUserAsync(User user) {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> Login(string email,string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.email.Equals(email)&&u.password.Equals(password));
            
        }
    }
}
