namespace ECommerce.Models;

public class UserProductsCart
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
