namespace ECommerce.Data.Interfaces;

public interface IUserRepository : IBaseRepository<ApplicationUser>
{
    Task<IEnumerable<Order>> GetUsersOrder(string userId);
    Task<IEnumerable<OrderProductDetails>> GetUsersOrderDetails(string orderId);

}