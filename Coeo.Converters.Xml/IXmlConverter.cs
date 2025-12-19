namespace Coeo.Converters.Xml
{
    public interface IXmlConverter
    {
        Task XmlFromFile<TLoadObject>(string filePath);
        Task Xml2FileAsync<TSaveObject>(string filePath, TSaveObject tSaveObject);
        Task<string> Xml2String<TSaveObject>(string filePath, TSaveObject tSaveObject);
    }
}