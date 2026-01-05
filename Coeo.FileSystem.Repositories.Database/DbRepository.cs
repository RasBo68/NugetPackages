
using Coeo.FileSystem.Repositories.Database.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Coeo.FileSystem.Repositories.Database
{
    public class DbRepository<TEntity>: IDbRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        private const string DATABASE_ERROR_MESSAGE = "Database operation failed!";
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

        public IQueryable<TEntity> ReadAll()
        {
            return ExecuteWithHandling(() =>
            {
                return _dbSet.AsNoTracking();
            });
        }
        public IQueryable<TEntity> GetAll()
        {
            return ExecuteWithHandling(() =>
            {
                return _dbSet;
            });
        }
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, READ, id.ToString()));

            return await ExecuteWithHandlingAsync(async () =>
            {
                return await _dbSet.FindAsync(id);
            }, id);
        }
        public async Task AddAsync(TEntity entity)
        {
            await ExecuteWithHandlingAsync(async () =>
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new object();
            }, entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any()) return;

            await ExecuteWithHandlingAsync(async () =>
            {
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                return new object();
            }, entities);
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            await ExecuteWithHandlingAsync(async () =>
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return new object();
            }, entity);
        }
        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any()) return;

            await ExecuteWithHandlingAsync(async () =>
            {
                _dbSet.UpdateRange(entities);
                await _context.SaveChangesAsync();
                return new object();
            }, entities);
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(ARGUMENT_OUT_OF_RANGE_EXCEPTION, DELETE, id.ToString()));

            await ExecuteWithHandlingAsync(async () =>
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                return new object();
            }, id);
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any()) return;

            await ExecuteWithHandlingAsync(async () =>
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
                return new object();
            }, entities);
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
                if (entity == null)
                    throw new DatabaseRowIsInUsageException(DATABASE_ERROR_MESSAGE, ex);
                else
                    throw new DatabaseRowIsInUsageException(DATABASE_ERROR_MESSAGE, ex, entity);
            }
            catch (Exception ex) // by exception while using addrange whole operation fails
            {
                if (entity == null)
                    throw new DatabaseException(DATABASE_ERROR_MESSAGE, ex);
                else
                    throw new DatabaseException(DATABASE_ERROR_MESSAGE, ex, entity);
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
                if (entity == null)
                    throw new DatabaseRowIsInUsageException(DATABASE_ERROR_MESSAGE, ex);
                else
                    throw new DatabaseRowIsInUsageException(DATABASE_ERROR_MESSAGE, ex, entity);
            }
            catch (Exception ex)
            {
                if (entity == null)
                    throw new DatabaseException(DATABASE_ERROR_MESSAGE, ex);
                else
                    throw new DatabaseException(DATABASE_ERROR_MESSAGE, ex, entity);
            }
        }

    }
}
