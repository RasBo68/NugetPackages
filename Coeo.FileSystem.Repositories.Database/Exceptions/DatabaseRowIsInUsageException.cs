namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class DatabaseRowIsInUsageException : Exception
    {
        public object? AdditionalInformation { get; set; }

        public DatabaseRowIsInUsageException() : base()
        {
        }
        public DatabaseRowIsInUsageException(string message)
            : base(message)
        {
        }
        public DatabaseRowIsInUsageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public DatabaseRowIsInUsageException(string message, Exception innerException, object additionalInformation)
            : base(message, innerException)
        {
            AdditionalInformation = additionalInformation;
        }
    }
}
