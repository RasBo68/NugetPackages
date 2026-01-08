
namespace Coeo.FileSystem.Repositories.Database
{
    public interface IDbRepository<TEntity>
    {
        Task AddAsync(TEntity entity, CancellationToken? cancellationToken = null);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
        Task DeleteAsync(int id, CancellationToken? cancellationToken = null);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity?> GetByIdAsync(int id, CancellationToken? cancellationToken = null);
        IQueryable<TEntity> ReadAllQuery();
        Task UpdateAsync(TEntity entity, CancellationToken? cancellationToken = null);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null);
    }
}