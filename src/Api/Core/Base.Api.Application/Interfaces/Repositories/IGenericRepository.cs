using Base.Api.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    // Insert Methods
    int Add(TEntity entity);
    Task<int> AddAsync(TEntity entity);
    int Add(IEnumerable<TEntity> entities);
    Task<int> AddAsync(IEnumerable<TEntity> entities);


    // Update Methods
    int Update(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);


    // Delete Methods
    int Delete(Guid id);
    Task<int> DeleteAsync(Guid id);
    int Delete(TEntity entity);
    Task<int> DeleteAsync(TEntity entity);
    bool DeleteRange(Expression<Func<TEntity, bool>> predicate);
    Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);


    // AddOrUpdate Methods
    int AddOrUpdate(TEntity entity);
    Task<int> AddOrUpdateAsync(TEntity entity);


    // Get Methods
    IQueryable<TEntity> AsQueryable();

    Task<List<TEntity>> GetAll(bool noTracking = true);

    Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);


    // Bulk Process Methods
    Task BulkAdd(IEnumerable<TEntity> entities);
    Task BulkUpdate(IEnumerable<TEntity> entities);
    Task BulkDeleteById(IEnumerable<Guid> ids);
    Task BulkDelete(Expression<Func<TEntity, bool>> predicate);
    Task BulkDelete(IEnumerable<TEntity> entities);

}
