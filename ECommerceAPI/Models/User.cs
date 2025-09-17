using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class User
    {
        public int id { get; set; } 
        public string username { get; set; } 
        public string email { get; set; } 
        public string password { get; set; } 
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public  Role  role{ get; set; } // e.g., "Customer", "Admin"
        public int CartId { get; set; } // Foreign key to the Cart  
        [JsonIgnore]
        public virtual Cart? Cart { get; set; } // Navigation property to the user's cart
        [JsonIgnore]
        public virtual List<Order>? Orders { get; set; } // Navigation property to the user's orders
    }

    public enum Role
    {
        Customer,
        Seller,
        Admin
    }
}
