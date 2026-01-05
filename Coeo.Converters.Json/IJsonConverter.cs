namespace Coeo.Converters.Json
{
    public interface IJsonConverter
    {
        T ConvertJsonStringToObject<T>(string jsonString);
        string ConvertObjectToJsonString(object entity);
    }
}