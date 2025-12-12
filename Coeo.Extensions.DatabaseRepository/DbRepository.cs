using Microsoft.EntityFrameworkCore;

namespace Coeo.Extensions.DatabaseRepository
{
    public class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        private readonly string _argumentOutOfRangeExceptionMessage = "An attempt was made to {0} the entity with id {1}.";
        private readonly string _read = "read";
        private readonly string _delete = "delete";

        // ############################################# Wichtiger Hinweis: Warum kein Lock? #############################################
        // 
        // 1)
        // In der Regel ist es nicht notwendig, explizit ein Lock in einem Repository einzuführen, wenn du Entity Framework Core (EF Core)
        // verwendest, da EF Core selbst Mechanismen zur Verwaltung der Datenbanktransaktionen und Konkurrenzsteuerung bietet. 
        //
        // 2)
        // IQueryable: Ein IQueryable lädt die Elemente nicht in den Arbeitsspeicher. Es wird lediglich ein Query-Baum aufgestellt.
        // UND sobald die Methode ToList() oder ToListAsync() (oder ähnliche) ausgeführt wird, wird der gesamte vorher erstellte Query-
        // Baum zu einem verknüpft und an die Datenquelle z.B. Datenbank als eine Query versendet und man lädt das Endresultat in den 
        // Arbeitsspeicher --> Effizienzverbesserung zu IEnumarable, das würde direkt alle Db Elemente in den lokalen Speicher laden.
        // ############################################# Wichtiger Hinweis: Warum kein Lock? #############################################

        public DbRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking(); // AsNoTracking: EF trackt veränderungen nicht -> Lesezugriff
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(_argumentOutOfRangeExceptionMessage, _read, id.ToString()));

            return await _dbSet.FindAsync(id) ?? Activator.CreateInstance<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            if (entities.ToList().Count == 0)
                return;

            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(string.Format(_argumentOutOfRangeExceptionMessage, _delete, id.ToString()));

            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            if (entities.ToList().Count==0)
                return;

            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
