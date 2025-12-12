using Newtonsoft.Json;

namespace Coeo.Extensions.JsonHelper
{
    public class JsonConverter : IJsonConverter
    {
        public string Convert2JsonString(object entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return JsonConvert.SerializeObject(entity);
        }

        public T Convert2Entity<T>(string jsonString)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(jsonString);

            return JsonConvert.DeserializeObject<T>(jsonString) ?? Activator.CreateInstance<T>();
        }
    }
}
