namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class FileException : Exception
    {
        public object? AdditionalInformation;
        public FileException(): base()
        {
        }
        public FileException(string message)
            : base(message)
        {
        }
        public FileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public FileException(string message, Exception innerException, object additionalInformation)
            : base(message, innerException)
        {
            AdditionalInformation = additionalInformation;
        }
    }
}
