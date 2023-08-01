using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get;set;} = string.Empty;
        public string LastName {get;set;} = string.Empty;
        public string Address {get;set;} = string.Empty;
         public byte[] ProfilePicture {get;set;} = null!;
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Order> Orders {get; set;} = new List<Order>();
        public IEnumerable<UserProductsCart> UsersProductsCarts { get; set; } = new HashSet<UserProductsCart>();
    }
}