namespace ECommerce.Data.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(string[] includes = null);
    Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[] includes = null);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int take, int skip);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int? take, int? skip,
        Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    T Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    IEnumerable<T> AddRange(IEnumerable<T> entities);
    IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    void DeleteRange(IEnumerable<T> entities);
}