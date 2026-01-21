namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class DatabaseRowIsInUsageException : Exception
    {
        public DatabaseRowIsInUsageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
