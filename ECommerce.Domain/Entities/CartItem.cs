using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }
        
        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}