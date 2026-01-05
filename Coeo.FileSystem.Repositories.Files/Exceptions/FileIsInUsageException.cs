namespace Coeo.FileSystem.Repositories.Files.Exceptions
{
    public class FileIsInUsageException : Exception
    {
        public object? AdditionalInformation { get; set; }

        public FileIsInUsageException() : base()
        {
        }
        public FileIsInUsageException(string message)
            : base(message)
        {
        }
        public FileIsInUsageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public FileIsInUsageException(string message, Exception innerException, object additionalInformation)
            : base(message, innerException)
        {
            AdditionalInformation = additionalInformation;
        }
    }
}
