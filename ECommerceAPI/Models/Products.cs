namespace ECommerceAPI.Models
{
    public class Product
    {
        public int id { get; set; } // Unique identifier for the product
        public string name { get; set; } // Name of the product
        public string description { get; set; } // Description of the product
        public decimal price { get; set; } // Price of the product
        public int stock { get; set; } // Available stock quantity
        public string category { get; set; } // Category of the product (e.g., Electronics, Clothing)
    }





}
