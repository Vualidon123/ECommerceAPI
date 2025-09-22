using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;  
        public ProductController(ILogger<ProductController> logger,ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        [Authorize]
        [HttpGet]   
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Models.Product product)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.id }, product);
        }
        [Authorize(Roles = "Admin,Seller")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Models.Product product)
        {
            
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetProductByIdAsync(product.id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            await _productService.UpdateProductAsync(product);
            return Ok();
        }
        [Authorize(Roles = "Admin,Seller")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            await _productService.DeleteProductAsync(id);
            return Ok();
        }
        
        [HttpGet("input")]
        public async Task<IActionResult> SearchProducts(string input)
        {
            var products = await _productService.SearchProduct(input);
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
    }
}
