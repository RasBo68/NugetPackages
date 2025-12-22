namespace Coeo.Converters.Json
{
    public interface IJsonConverter
    {
        T ConvertJsonString2Object<T>(string jsonString);
        string ConvertObject2JsonString(object entity);
    }
}