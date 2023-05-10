namespace ECommerce.Models
{
    public class AuthModel
    {
        public string Messages { get; set; }
        public bool isAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}