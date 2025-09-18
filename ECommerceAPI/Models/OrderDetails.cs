using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class OrderDetails
    {
        public int id { get; set; } // Unique identifier for the order detail
        public int orderId { get; set; } // ID of the associated order  
        public int productId { get; set; } // ID of the product in the order
        //public decimal price { get; set; } // Price of the product at the time of order
        public int quantity { get; set; } // Quantity of the product ordered
        [JsonIgnore]
        public virtual Order? Order { get; set; } // Navigation property to the Order
        [JsonIgnore]
        public virtual Product? Product { get; set; } // Navigation property to the Product
    }
}
