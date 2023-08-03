namespace ECommerce.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ICategoryRepository Categories { get; private set; }
    public IProductRepository Products { get; private set; }
    public IOrderProductDetailsRepository OrderDetails { get; private set; }
    public IOrderRepository Orders { get; private set; }
    public IUserRepository Users { get; private set; }
    public IReviewRepository Reviews { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Categories = new CategoryRepository(_context);
        Products = new ProductRepository(_context);
        OrderDetails = new OrderDetailsRepository(_context);
        Orders = new OrderRepository(_context);
        Users = new UserRepository(_context);
        Reviews = new ReviewRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
