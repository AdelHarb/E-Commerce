using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get;set;} = string.Empty;
        public string LastName {get;set;} = string.Empty;
        public IEnumerable<string> Address {get;set;} = new List<string>();
        public string Code { get; set; } = string.Empty;
        // public byte[] ProfilePicture {get;set;} = null!;
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Order> Orders {get; set;} = new List<Order>();
        public IEnumerable<Review> Reviews { get; set; } = null!;
        public IEnumerable<UserProductsCart> UsersProductsCarts { get; set; } = new HashSet<UserProductsCart>();
    }
}