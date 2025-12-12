namespace Coeo.Extensions.JsonHelper
{
    public interface IJsonConverter
    {
        T Convert2Entity<T>(string jsonString);
        string Convert2JsonString(object entity);
    }
}