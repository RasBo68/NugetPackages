
namespace XmlHandling
{
    public interface IXmlService
    {
        Task LoadXmlFileAsync<TLoadObject>(string filePath);
        Task SaveXmlFileAsync<TSaveObject>(string filePath, TSaveObject tSaveObject, List<string> linesToDelete);
    }
}