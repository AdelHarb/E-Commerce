namespace ECommerce.Data.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<int> GetLastUserOrder(string userId);
        Task<IEnumerable<Order>>? GetOrdersWithData();
        Task<Order>? GetOrderWithProducts();


    }