using ECommerceAPI.Models;
using ECommerceAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAPI.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthService(UserRepository userRepository, IConfiguration configuration)
        {

            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<string> Register(DTOs.RegisterRequest request)
        {
            var user = new Models.User
            {
                email = request.email,
                password = request.password,
                address = request.address,
                phoneNumber = request.phoneNumber,
                username = request.username,
                CartId = 0,
                Cart = null,
                Orders = null,
                role = Role.Customer
            };
            await _userRepository.AddUserAsync(user);
            return GenerateJwtToken(user);
            
        }
        public  async Task<string?> Login(string email, string password)
        {
           var user= await _userRepository.Login(email, password);

           return GenerateJwtToken(user);
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.username),
            new Claim("role", user.role.ToString()),
            new Claim("userId", user.id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

