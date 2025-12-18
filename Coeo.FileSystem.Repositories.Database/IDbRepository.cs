namespace Coeo.FileSystem.Repositories.Database
{
    public interface IDbRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetAll();
        Task<TEntity?> GetByIdAsync(int id);
        IQueryable<TEntity> ReadAll();
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}