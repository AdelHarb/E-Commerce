namespace ECommerce.Models;

public class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public IEnumerable<Review> Reviews { get; set; } = null!;
    public IEnumerable<OrderProductDetails> OrderProductDetails { get; set; } = new List<OrderProductDetails>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliverDate { get; set; } = null;
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}