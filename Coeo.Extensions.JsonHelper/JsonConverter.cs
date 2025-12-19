
using System.Text.Json;

namespace Coeo.Extensions.JsonHelper
{
    public class JsonConverter : IJsonConverter
    {
        private const string JSON_STRING_EMPTY_OR_WHITESPACE = "Json string is empty or just whitespace.";
        private const string DESERIALIZING_ERROR = "Error during JSON deserialization.";
        private const string DESERIALIZING_NULL_ERROR = "Deserialization resulted in null.";

        public string Convert2JsonString(object entity)
        {
            return JsonSerializer.Serialize(entity);
        }

        public T Convert2Entity<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) || string.IsNullOrWhiteSpace(jsonString))
                throw new InvalidOperationException(JSON_STRING_EMPTY_OR_WHITESPACE);

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                T? val = JsonSerializer.Deserialize<T>(jsonString, options);
                if (val == null)
                    throw new InvalidOperationException(DESERIALIZING_NULL_ERROR);

                return val;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(DESERIALIZING_ERROR, ex);
            }
        }
    }
}
