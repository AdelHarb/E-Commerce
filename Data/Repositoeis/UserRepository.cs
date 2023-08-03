namespace ECommerce.Data.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetUsersOrder(string userId)
        {
            return _context.Orders.Include(o => o.OrderProductDetails)
                                    .ThenInclude(o => o.Product)
                                    .Where(o => o.UserId == userId);
        }

        public async Task<IEnumerable<OrderProductDetails>> GetUsersOrderDetails(string orderId)
        {
            return _context.OrderProductDetails.Include(x => x.Product)
                .Where(x => x.OrderId == orderId);
        }
    }
}