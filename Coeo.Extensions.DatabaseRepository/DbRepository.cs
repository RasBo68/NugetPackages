using Microsoft.EntityFrameworkCore;

namespace Coeo.Extensions.DatabaseRepository
{
    public class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        private const string ARGUMENT_OUT_OF_RANGE_EXCEPTION = "An attempt was made to {0} the entity with id {1}.";
        private const string READ = "read";
        private const string DELETE = "delete";

        // ############################################# Important Note: Why No Lock? #############################################
        // 
        // 1)
        // Generally, it is not necessary to explicitly introduce a lock in a repository when using Entity Framework Core (EF Core),
        // because EF Core itself provides mechanisms for handling database transactions and concurrency control.
        //
        // 2)
        // IQueryable: An IQueryable does not load the elements into memory immediately. It only constructs a query tree.
        // AND once methods like ToList() or ToListAsync() (or similar) are executed, the entire previously constructed query tree
        // is compiled and sent to the data source (e.g., the database) as a single query, loading the final result into memory.
        // This provides an efficiency improvement over IEnumerable, which would immediately load all database elements into local memory.
        // ############################################# Important Note: Why No Lock? #############################################

        public DbRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking(); // AsNoTracking: EF does not track changes -> read
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, READ, id.ToString()));

            return await _dbSet.FindAsync(id) ?? Activator.CreateInstance<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.ToList().Count == 0)
                return;

            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, DELETE, id.ToString()));

            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.ToList().Count==0)
                return;

            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
