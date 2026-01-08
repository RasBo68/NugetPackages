
namespace Coeo.FileSystem.Repositories.Database
{
    public interface IDbRepository<TEntity>
    {
        Task AddAsync(TEntity entity, CancellationToken? cancellationToken);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken);
        Task DeleteAsync(int id, CancellationToken? cancellationToken);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken);
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity?> GetByIdAsync(int id, CancellationToken? cancellationToken);
        IQueryable<TEntity> ReadAllQuery();
        Task UpdateAsync(TEntity entity, CancellationToken? cancellationToken);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken);
    }
}