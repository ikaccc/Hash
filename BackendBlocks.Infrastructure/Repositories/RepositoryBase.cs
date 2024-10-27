using System.Linq.Expressions;
using BackendBlocks.Core.Common;
using BackendBlocks.Core.Contracts.Persistence;
using BackendBlocks.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BackendBlocks.Infrastructure.Repositories;

public class RepositoryBase<T>(HashDBContext _hashDBContext) : IAsyncRepository<T> where T : EntityBase
{
    protected readonly DbSet<T> _dbSet = _hashDBContext.Set<T>();

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeString = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();   
        }

        if (!string.IsNullOrWhiteSpace(includeString))
        {
            query = query.Include(includeString);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _hashDBContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _hashDBContext.Entry(entity).State = EntityState.Modified;
        await _hashDBContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _hashDBContext.SaveChangesAsync();
    }
}
