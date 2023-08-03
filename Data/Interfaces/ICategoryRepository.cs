namespace ECommerce.Data.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Product>>? GetByIdWithProductsAsync(int id);

        Task<IEnumerable<Category>>? GetAllWithProductsAsync();
    }
}