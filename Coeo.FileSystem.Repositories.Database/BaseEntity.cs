
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace Coeo.FileSystem.Repositories.Database
{
    public abstract class BaseEntity
    {
        // primary key
        [Key]public int Id { get; set; }

        // concurrency token -> So there appears an db concurrency exception when two users try to update the same entity at the same time
        [Timestamp] public byte[] RowVersion { get; set; } = new byte[0];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        /* In Combination with this in the DbContext:
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
        */
    }
}
