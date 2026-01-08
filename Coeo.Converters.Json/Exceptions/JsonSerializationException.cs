
namespace Coeo.Converters.Json.Exceptions
{
    public class JsonSerializationException : Exception
    {
        public JsonSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
