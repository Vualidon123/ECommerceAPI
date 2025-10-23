using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        [JsonIgnore]
        public virtual User? User { get; set; }
        
        [JsonIgnore]
        public virtual List<CartItem>? CartItems { get; set; }
    }
}