
using System.Text.Json;

namespace Coeo.Extensions.JsonHelper
{
    public class JsonConverter : IJsonConverter
    {
        public string Convert2JsonString(object entity)
        {
            return JsonSerializer.Serialize(entity);
        }

        public T Convert2Entity<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) || string.IsNullOrWhiteSpace(jsonString))
                throw new InvalidOperationException("Json string is empty or just whitespace.");

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                T val = JsonSerializer.Deserialize<T>(jsonString, options);
                if (val == null)
                {
                    throw new InvalidOperationException("Deserialization resulted in null.");
                }
                return val;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Error during JSON deserialization", ex);
            }
        }
    }
}
