using Data.Contexts;
using Domain.Extensions;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
{
    Task<RepositoryResults<bool>> AddAsync(TEntity entity);
    Task<RepositoryResults<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResults<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResults<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] include);
    Task<RepositoryResults<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] include);
    Task<RepositoryResults<TModel>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResults<bool>> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
{

    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _table;
    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResults<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResults<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public virtual async Task<RepositoryResults<IEnumerable<TModel>>> GetAllAsync
        (
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] include
        )
    {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);
        if (include != null && include.Length != 0)
            foreach (var includeProperty in include)
                query = query.Include(includeProperty);
        if (sortBy != null)
        {
            if (orderByDescending)
                query = query.OrderByDescending(sortBy);
            else
                query = query.OrderBy(sortBy);
        }


        var entities = await query.ToListAsync();

        var results = entities.Select(entity => entity.MapTo<TModel>());
        return new RepositoryResults<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = results };
    }


    public virtual async Task<RepositoryResults<IEnumerable<TSelect>>> GetAllAsync<TSelect>
    (
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] include
    )
    {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);
        if (include != null && include.Length != 0)
            foreach (var includeProperty in include)
                query = query.Include(includeProperty);
        if (sortBy != null)
        {
            if (orderByDescending)
                query = query.OrderByDescending(sortBy);
            else
                query = query.OrderBy(sortBy);
        }


        var entities = await query.Select(selector).ToListAsync();

        var results = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepositoryResults<IEnumerable<TSelect>> { Succeeded = true, StatusCode = 200, Result = results };
    }

    public virtual async Task<RepositoryResults<TModel>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (includes != null && includes.Length != 0)
            foreach (var includeProperty in includes)
                query = query.Include(includeProperty);


        var entity = await _table.FirstOrDefaultAsync(where);
        if (entity == null)
            return new RepositoryResults<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

        var result = entity.MapTo<TModel>();
        return new RepositoryResults<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepositoryResults<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return !exists
            ? new RepositoryResults<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found" }
            : new RepositoryResults<bool> { Succeeded = true, StatusCode = 200 };
    }

    public virtual async Task<RepositoryResults<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResults<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
    public virtual async Task<RepositoryResults<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResults<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResults<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
}
