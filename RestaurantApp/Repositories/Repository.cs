using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Repositories;

public class Repository<T> : IRepository<T> where T : class
{

    protected ApplicationDbContext _applicationDbContext { get; set; }
    private DbSet<T> _dbSet { get; set; }

    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _dbSet = applicationDbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
       return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
    { 
        IQueryable<T> query = _dbSet;
        if (options.HasWhere)
        {
            query = query.Where(options.Where);
        }
        
        if (options.HasOrderBy)
        {
            query = query.OrderBy(options.OrderBy);
        }
        
        foreach (string include in options.GetIncludes())
        {
            query = query.Include(include);
        }

        var key = _applicationDbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
        string primaryKeyName = key?.Name;
        return await query.FirstOrDefaultAsync(q => EF.Property<int>(q, primaryKeyName) == id);
    }

    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}