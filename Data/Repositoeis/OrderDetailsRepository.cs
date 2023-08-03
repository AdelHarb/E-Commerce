namespace ECommerce.Data.Repositories;

public class OrderDetailsRepository : BaseRepository<OrderProductDetails>, IOrderProductDetailsRepository
    {
        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
        }


    public async Task<OrderProductDetails?> GetByCompositeId(int ProductId, int orderId)
    {
        return await _context.OrderProductDetails
            .Where(opd => opd.ProductId == ProductId && opd.OrderId == orderId)
            .FirstOrDefaultAsync();
    }

    // public async Task<IEnumerable<OrderProductDetails>> GetTopProducts()
    // {
    //     return await _context.OrderProductDetails
    // }
}