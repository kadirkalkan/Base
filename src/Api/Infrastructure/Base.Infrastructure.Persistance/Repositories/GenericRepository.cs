﻿using Base.Api.Application.Interfaces.Repositories;
using Base.Api.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;

    protected DbSet<TEntity> entity => _context.Set<TEntity>();

    public GenericRepository(DbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Insert Methods

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        await this.entity.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual int Add(TEntity entity)
    {
        this.entity.Add(entity);
        return _context.SaveChanges();
    }

    public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
    {
        if (entities != null && !entities.Any())
            return 0;

        await entity.AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual int Add(IEnumerable<TEntity> entities)
    {
        if (entities != null && !entities.Any())
            return 0;

        entity.AddRange(entity);
        return _context.SaveChanges();
    }

    #endregion

    #region Update Methods

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        this.entity.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        return await _context.SaveChangesAsync();
    }

    public virtual int Update(TEntity entity)
    {
        this.entity.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        return _context.SaveChanges();
    }

    #endregion

    #region Delete Methods

    public virtual Task<int> DeleteAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            this.entity.Attach(entity);
        }

        this.entity.Remove(entity);

        return _context.SaveChangesAsync();
    }

    public virtual Task<int> DeleteAsync(Guid id)
    {
        var entity = this.entity.Find(id);
        return DeleteAsync(entity);
    }

    public virtual int Delete(Guid id)
    {
        var entity = this.entity.Find(id);
        return Delete(entity);
    }

    public virtual int Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            this.entity.Attach(entity);
        }

        this.entity.Remove(entity);

        return _context.SaveChanges();
    }

    public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(entity.Where(predicate));
        return _context.SaveChanges() > 0;
    }

    public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(entity.Where(predicate));
        return await _context.SaveChangesAsync() > 0;
    }

    #endregion

    #region AddOrUpdate Methods

    public virtual Task<int> AddOrUpdateAsync(TEntity entity)
    {
        // check the entity with the id already tracked
        if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            _context.Update(entity);

        return _context.SaveChangesAsync();
    }

    public virtual int AddOrUpdate(TEntity entity)
    {
        if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            _context.Update(entity);

        return _context.SaveChanges();
    }

    #endregion

    #region Get Methods

    public virtual IQueryable<TEntity> AsQueryable() => entity.AsQueryable();
    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = entity.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        if (noTracking)
            query = query.AsNoTracking();

        return query;
    }


    public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
    }

    public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            query = query.Include(include);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (noTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
    {
        if (noTracking)
            return await entity.AsNoTracking().ToListAsync();

        return await entity.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        TEntity found = await entity.FindAsync(id);

        if (found == null)
            return null;

        if (noTracking)
            _context.Entry(found).State = EntityState.Detached;

        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            _context.Entry(found).Reference(include).Load();
        }

        return found;
    }

    public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = ApplyIncludes(query, includes);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync();

    }

    #endregion

    #region Bulk Methods

    public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
            return Task.CompletedTask;

        _context.RemoveRange(entity.Where(i => ids.Contains(i.Id)));
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(entity.Where(predicate));
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return Task.CompletedTask;

        entity.RemoveRange(entities);
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return Task.CompletedTask;

        foreach (var entityItem in entities)
        {
            entity.Update(entityItem);
        }

        return _context.SaveChangesAsync();
    }

    public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            await Task.CompletedTask;

        await entity.AddRangeAsync(entities);

        await _context.SaveChangesAsync();
    }

    #endregion

    #region SaveChanges Methods

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    #endregion


    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes != null)
        {
            foreach (var includeItem in includes)
            {
                query = query.Include(includeItem);
            }
        }

        return query;
    }

}