
using Coeo.Converters.Json.Exceptions;
using System.Text.Json;

namespace Coeo.Converters.Json
{
    public class JsonConverter : IJsonConverter
    {
        private const string JSON_STRING_EMPTY_OR_WHITESPACE = "Json string is empty or just whitespace.";
        private const string DESERIALIZATION_EXCEPTION = "Deserialization of the xmlContentString \n {0} \n failed.";
        private const string SERIALIZATION_EXCEPTION = "serialization of the object \n {0} \n failed.";

        public TObject ConvertJsonStringToObject<TObject>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) || string.IsNullOrWhiteSpace(jsonString))
                throw new ArgumentException(JSON_STRING_EMPTY_OR_WHITESPACE);

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<TObject>(jsonString, options)!;
            }
            catch (Exception ex)
            {
                throw new JsonDeserializationException(
                    string.Format(DESERIALIZATION_EXCEPTION, jsonString),
                    ex);
            }
        }
        public string ConvertObjectToJsonString(object entity)
        {
            try
            {
                return JsonSerializer.Serialize(entity);
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(
                    string.Format(SERIALIZATION_EXCEPTION, entity.ToString()),
                    ex);
            }
        }
    }
}
