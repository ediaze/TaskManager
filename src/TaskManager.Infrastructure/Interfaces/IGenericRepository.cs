using System.Linq.Expressions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Interfaces
{
    public interface IGenericRepository<TEntity> 
        where TEntity : class, IEntityWithIdGuid
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        Task<IList<TEntity>?> GetAllAsync();
        Task<IList<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<int?> UpdateAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
