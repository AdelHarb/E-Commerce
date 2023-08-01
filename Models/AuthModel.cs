using System.Text.Json.Serialization;

namespace ECommerce.Models
{
    public class AuthModel
    {
        public string Messages { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiration { get; set; } 

    }
}