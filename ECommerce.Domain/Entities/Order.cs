using System.Text.Json.Serialization;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        
        [JsonIgnore]
        public virtual User? User { get; set; }
        
        [JsonIgnore]
        public virtual List<OrderDetails>? OrderDetails { get; set; }
    }
}