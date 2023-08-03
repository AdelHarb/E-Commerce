namespace ECommerce.Data.Interfaces;

public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewByProduct (int productId);
        Task<IEnumerable<Review>> GetReviewsWithProductAndUser();
        Task<Review> GetByCompositeId(int ProductID, string userID);

    }