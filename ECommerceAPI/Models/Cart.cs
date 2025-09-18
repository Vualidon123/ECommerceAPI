using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Cart
    {
        public int id { get; set; } // Unique identifier for the cart item
        public int userId { get; set; } // ID of the user who owns the cart
        public int quantity { get; set; } // Quantity of the product in the cart

        // Navigation properties (optional, depending on your ORM setup)
        [JsonIgnore]
        public virtual User? User { get; set; } // Navigation property to the User

        
        public virtual List<CartItem>? CartItems { get; set; } // Items inside the cart
    }
}
