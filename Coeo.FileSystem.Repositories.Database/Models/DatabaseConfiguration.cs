
namespace Coeo.FileSystem.Repositories.Database.Models
{
    public class DatabaseConfiguration
    {
        public string ServerPath { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSqlAuthentification { get; set; } = false;
    }
}
