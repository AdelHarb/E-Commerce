namespace ECommerce.Data.Interfaces;

public interface IOrderProductDetailsRepository : IBaseRepository<OrderProductDetails>
    {
    // Task<IEnumerable<OrderProductDetails>> GetTopProducts();
    Task<OrderProductDetails?> GetByCompositeId(int ProductId, int orderId);
}