namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class XmlSerializationException : Exception
    {
        public XmlSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
