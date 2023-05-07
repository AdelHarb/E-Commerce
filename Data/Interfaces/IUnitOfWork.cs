namespace ECommerce.Data.Interfaces;

public interface IUnitOfWork : IDisposable  
{
    IBaseRepository<Category> Categories { get; }
    IBaseRepository<Product> Products { get; }
    int Complete();
}