
namespace Coeo.Converters.Json.Exceptions
{
    public class JsonDeserializationException:Exception
    {
        public JsonDeserializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
