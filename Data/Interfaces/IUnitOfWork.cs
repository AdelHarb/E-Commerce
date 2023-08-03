namespace ECommerce.Data.Interfaces;

public interface IUnitOfWork : IDisposable  
{
    // IProductRepository ProductRepository { get; }
    // ICategoryRepository CategoryRepository { get; }
    // IReviewRepository ReviewRepository { get; }
    // IUserRepository UserRepository { get; }
    // IOrderRepository OrderRepository { get; }
    // IOrderProductDetailsRepository OrderProductDetailsRepository { get; }
    int Complete();
}