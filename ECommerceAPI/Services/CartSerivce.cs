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
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }
        public async Task<Cart> GetCartByIdAsync(int id)
        {
            return await _cartRepository.GetCartAsyncById(id);
        }
        public async Task AddCartAsync(Cart cart)
        {
            await _cartRepository.AddCartAsync(cart);
        }
        public async Task UpdateCartAsync(Cart cart)
        {
            await _cartRepository.UpdateCartAsync(cart);
        }
        public async Task DeleteCartAsync(int cartId)
        {
            await _cartRepository.DeleteCartAsync(cartId);
        }
    }
}
