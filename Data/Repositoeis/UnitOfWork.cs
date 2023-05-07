namespace ECommerce.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;


    public IBaseRepository<Category> Categories {get; private set;}

    public IBaseRepository<Product> Products {get; private set;}
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Categories = new BaseRepository<Category>(_context);
        Products = new BaseRepository<Product>(_context);
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