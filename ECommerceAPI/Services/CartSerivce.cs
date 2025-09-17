using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CartSerivce
    {
        private readonly CartRepository _cartRepository;
        public CartSerivce(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        /*public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _cartRepository.();
        }*/
    }
}
