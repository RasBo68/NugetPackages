
using System.Xml.Serialization;

namespace Coeo.Converters.Xml
{
    public interface IXmlConverter
    {
        TLoadObject ConvertXmlStringToObject<TLoadObject>(string xmlContentString);
        string ConvertObjectToXmlString<TSaveObject>(TSaveObject tSaveObject);
    }
}