
using System.Text;

namespace Coeo.Converters.Xml
{
    public interface IXmlConverter
    {
        string ConvertObjectToXmlString<TSaveObject>(TSaveObject tSaveObject, Dictionary<string, string>? xmlNamespaces = null, bool setXmlDeclaration = false,
            bool showXmlVersion = true);
        TLoadObject ConvertXmlStringToObject<TLoadObject>(string xmlContentString);
    }
}