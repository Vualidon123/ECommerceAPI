using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartSerivce _cartService;
        public CartController(CartSerivce cartService)
        {
            _cartService = cartService;
        }
        [Authorize(Roles = "User")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] Models.Cart cart)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            // Upsert behavior: if a cart exists for this user, update it instead of inserting a second row
            var existing = await _cartService.GetCartByUserIdAsync(cart.userId);
            if (existing != null)
            {
                cart.id = existing.id;
                await _cartService.UpdateCartAsync(cart);
                return Ok(cart);
            }

            await _cartService.AddCartAsync(cart);
            return CreatedAtAction(nameof(GetCartByUserId), new { userId = cart.userId }, cart);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] Models.Cart cart)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var existingCart = await _cartService.GetCartByIdAsync(cart.id);
            if (existingCart == null)
            {
                return NotFound();
            }
            await _cartService.UpdateCartAsync(cart);
            return Ok();
        }
        [Authorize]
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            await _cartService.DeleteCartAsync(cartId);
            return NoContent();
        }

    }
}