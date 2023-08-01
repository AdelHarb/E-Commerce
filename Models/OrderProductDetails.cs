namespace ECommerce.Models;

public class OrderProductDetails
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public Product Product { get; set; } = null!;
    public Order Order { get; set; } = null!;
}