using ECommerceAPI.Data;

namespace ECommerceAPI.Repositories
{
    public class CartRepository
    {
        private readonly ECommerceDbContext _context;
        public CartRepository(ECommerceDbContext context) {
            _context = context;
        }


    }
}
