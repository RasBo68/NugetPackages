
using System.Xml.Serialization;

namespace Coeo.Converters.Xml
{
    public interface IXmlConverter
    {
        TLoadObject ConvertXmlContentString2Object<TLoadObject>(string xmlContentString);
        string ConvertObject2XmlContentString<TSaveObject>(TSaveObject tSaveObject);
    }
}