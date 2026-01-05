
using System.ComponentModel.DataAnnotations;

namespace Coeo.FileSystem.Repositories.Database
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        [Timestamp] public byte[] RowVersion { get; set; } = new byte[0];
    }
}
