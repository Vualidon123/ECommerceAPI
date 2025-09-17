namespace ECommerceAPI.DTOs
{
    public class LoginRequest
    {
        /*public int id { get; set; }*/
        
        public string email { get; set; }

        public string password { get; set; }
    }
    public class User
    {
        public int id { get; set; }
        public int role { get; set; }
    }
    public class RegisterRequest    
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
    }
}
