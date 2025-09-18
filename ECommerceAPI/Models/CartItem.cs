using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class CartItem
    {
        public int id { get; set; } // Unique identifier for the cart item
        public int cartId { get; set; } // FK to Cart
        public int productId { get; set; } // FK to Product
        public int quantity { get; set; } // Quantity of the product
        
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
