namespace ECommerce.Data.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

    public async Task<int> GetLastUserOrder(string userId)
    {
        var order = await _context.Orders
            .Where(x => x.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        return order?.Id ?? 0;
    }

    public async Task<IEnumerable<Order>?> GetOrdersWithData()
    {
        return await _context.Orders.Include(x => x.User)
            .Include(x => x.OrderProductDetails)
                .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithProducts()
    {
        return await _context.Orders
        .Include(x => x.OrderProductDetails)
            .ThenInclude(x => x.Product)
        .FirstOrDefaultAsync();
    }
}