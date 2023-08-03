namespace ECommerce.Data.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    public async Task<IEnumerable<Category>>? GetAllWithProductsAsync()
    {
        return _context.Categories.Include(c => c.Products);   
    }

    public async Task<IEnumerable<Product>>? GetByIdWithProductsAsync(int id)
    {
        return _context.Products.Include(x => x.Reviews)
        .Include(x => x.Categories)
        .Where(x => x.Categories.Any(x => x.Id == id));
    }
}