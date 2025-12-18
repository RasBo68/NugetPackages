namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(): base()
        {
        }
        public DatabaseException(string message)
            : base(message)
        {
        }
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class DatabaseException<TInfo> : DatabaseException
    {
        public TInfo? AdditionalInformation;

        public DatabaseException(string message, Exception innerException, TInfo additionalInformation)
            : base(message, innerException)
        {
            AdditionalInformation = additionalInformation;
        }
    }
}
