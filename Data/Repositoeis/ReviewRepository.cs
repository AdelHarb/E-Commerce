namespace ECommerce.Data.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

    public async Task<Review> GetByCompositeId(int ProductID, string userID)
    {
        return await _context.Reviews.Where(x => x.ProductId == ProductID && x.UserId == userID)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewByProduct(int productId)
    {
        return await _context.Reviews.Where(x => x.ProductId == productId).ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsWithProductAndUser()
    {
        return await _context.Reviews.Include(x=>x.User).Include(x=> x.Product).ToListAsync();
    }
}