using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int id { get; set; } // Unique identifier for the order
        public int userId { get; set; } // ID of the user who placed the order
        public DateTime orderDate { get; set; } // Date when the order was placed
        public OrderStatus status { get; set; } // Status of the order (e.g., "Pending", "Shipped", "Delivered")
        public decimal totalAmount { get; set; } // Total amount for the order
        [JsonIgnore]
        public virtual User? User { get; set; } // Navigation property to the user who placed the order
        [JsonIgnore]
        public virtual List<OrderDetails>? OrderDetails { get; set; } // Navigation property to the order details
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
}
