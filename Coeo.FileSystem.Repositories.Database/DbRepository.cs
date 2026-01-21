
using Coeo.FileSystem.Repositories.Database.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace Coeo.FileSystem.Repositories.Database
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
        //
        // 3) Update best practise always: Load → Modify → Save
        //    Load with GetAllQuery() or GetByIdAsync(...), modify the entity in memory, then save changes with SaveAsync(...)
        //    _dbset.Update(entity) is not necessary, because the DbContext is already tracking the entity.
        //    Disadvantages of using _dbset.Update(entity):
        //    - all properties were marked as modified, even if only a few were changed -> Lead to concurrency problems
        //    - performance overhead, especially with large entities or frequent updates.
        // ############################################# Important Note: Why No Lock? #############################################

        public DbRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> ReadAllQuery()
        {
            return ExecuteWithHandling(() =>
            {
                return _dbSet.AsNoTracking();
            });
        }
        public IQueryable<TEntity> GetAllQuery()
        {
            return ExecuteWithHandling(() =>
            {
                return _dbSet;
            });
        }
        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken? cancellationToken = null)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, READ, id.ToString()));

            return await ExecuteWithHandlingAsync(async () =>
            {
                return await _dbSet.FindAsync(id, cancellationToken ?? CancellationToken.None);
            }, id);
        }
        public async Task AddAsync(TEntity entity, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandlingAsync(async () =>
            {
                await _dbSet.AddAsync(entity, cancellationToken ?? CancellationToken.None);
                await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
                return new object();
            }, entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null)
        {
            if (!entities.Any()) return;

            await ExecuteWithHandlingAsync(async () =>
            {
                await _dbSet.AddRangeAsync(entities, cancellationToken ?? CancellationToken.None);
                await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
                return new object();
            }, entities);
        }
        public async Task DeleteAsync(int id, CancellationToken? cancellationToken = null)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, DELETE, id.ToString()));

            await ExecuteWithHandlingAsync(async () =>
            {
                var entity = await GetByIdAsync(id, cancellationToken ?? CancellationToken.None);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
                }
                return new object();
            }, id);
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken? cancellationToken = null)
        {
            if (!entities.Any()) return;

            await ExecuteWithHandlingAsync(async () =>
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
                return new object();
            }, entities);
        }
        public async Task SaveAsync(CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandlingAsync(async () =>
            {
                await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
                return new object();
            });
        }


        private async Task<TOut> ExecuteWithHandlingAsync<TOut>(Func<Task<TOut>> action, object? entity = default)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is NotSupportedException || ex is InvalidCastException)
            {
                throw;
            }
            // Concurrency exception handling: e.g., when a row is being used by another process, by exception while using addrange whole operation fails
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DatabaseRowIsInUsageException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }
        private TOut ExecuteWithHandling<TOut>(Func<TOut> action, object? entity = default)
        {
            try
            {
                return action();
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is NotSupportedException || ex is InvalidCastException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DatabaseRowIsInUsageException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

    }
}
