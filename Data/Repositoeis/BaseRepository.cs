namespace ECommerce.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
         
        return entities;
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        if(predicate is null)
            return _context.Set<T>().CountAsync();
        else
            return _context.Set<T>().CountAsync(predicate);
    }

    public void Delete(T entity) =>  _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if(includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int take, int skip)
    {
        IQueryable<T> query = _context.Set<T>();

        return await query.Where(predicate).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate,
                                                    int? take, int? skip,
                                                    Expression<Func<T, object>> orderBy = null,
                                                    string orderByDirection = "ASC")
    {
        IQueryable<T> query = _context.Set<T>().Where(predicate);

        if(take.HasValue)
            query = query.Take(take.Value);

        if(skip.HasValue)
            query = query.Skip(skip.Value);
        
        if(orderBy is not null)
        {
            if(orderByDirection is OrderBy.Ascending)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
        }
        return await query.ToListAsync();
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if(includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync(string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if(includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        return Enumerable.Empty<T>();
    }


    public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
        return entities;
    }
}
