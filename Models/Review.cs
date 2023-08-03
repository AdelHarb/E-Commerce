namespace ECommerce.Models;

public class Review
{
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int ProductId { get; set; } 
    public string UserId { get; set; } = string.Empty;
    public int OrderId { get; set; } 
    public Product Product { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public Order Order { get; set; } = null!;
}