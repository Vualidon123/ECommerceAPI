using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        [JsonIgnore]
        public virtual Order? Order { get; set; }
        
        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}