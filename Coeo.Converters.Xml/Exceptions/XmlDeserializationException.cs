namespace Coeo.FileSystem.Repositories.Database.Exceptions
{
    public class XmlDeserializationException : Exception
    {
        public XmlDeserializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
