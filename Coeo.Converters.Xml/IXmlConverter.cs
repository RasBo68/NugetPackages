
namespace Coeo.Converters.Xml
{
    public interface IXmlConverter
    {
        Task<TLoadObject> ConvertXmlString2Object<TLoadObject>(string filePath);
        Task<string> ConvertObject2XmlString<TSaveObject>(string filePath, TSaveObject tSaveObject);
    }
}