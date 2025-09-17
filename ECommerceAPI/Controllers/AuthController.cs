using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly AuthService _authService;
        public AuthController( AuthService authService)
        {
          
            _authService = authService;
            
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var token =await _authService.Login(email, password);
            if (token != null)
            {
                
                return Ok(token);
            }
            return Unauthorized("Invalid email or password");
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromQuery] DTOs.RegisterRequest newuser)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            
            var ex = await  _authService.Register(newuser);
            if (ex != null)
            {
                return Ok(ex);
            }
            return BadRequest("UserExisted");
        }
        
    }
}
