using System.Data;
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

            return JsonSerializer.Deserialize<T>(jsonString) ?? Activator.CreateInstance<T>();
        }
    }
}
