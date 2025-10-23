using System.Text.Json.Serialization;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Role Role { get; set; }
        public int CartId { get; set; }
        
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }
        
        [JsonIgnore]
        public virtual List<Order>? Orders { get; set; }
    }
}