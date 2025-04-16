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

    public Task<T> GetByIdAsync(int id, QueryOptions<T> options)
    {
        throw new NotImplementedException();
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