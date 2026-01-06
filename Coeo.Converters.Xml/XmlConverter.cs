
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Coeo.Converters.Xml
{
    public class XmlConverter : IXmlConverter
    {
        private const string IDENT_CHARS = "\t";
        private const string NEW_LINE_CHARS = "\n";
        private const string XML_VERSION_STRING = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        private const string NIL_KEY = "nil";
        private const string XMLNS_XSI_VALUE = "http://www.w3.org/2001/XMLSchema-instance";

        public TLoadObject ConvertXmlStringToObject<TLoadObject>(string xmlContentString)
        {
            using MemoryStream stream = new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes(xmlContentString));
            var serializer = new XmlSerializer(typeof(TLoadObject));
            TLoadObject? val = (TLoadObject?)serializer.Deserialize(stream);
            return (val != null) ? val : Activator.CreateInstance<TLoadObject>();
        }
        public string ConvertObjectToXmlString<TSaveObject>(
            TSaveObject tSaveObject,
            Dictionary<string, string>? xmlNamespaces = null,
            bool setXmlDeclaration = false, 
            bool showXmlVersion = true)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TSaveObject));
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            if (xmlNamespaces == null)
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            else
            {
                foreach (var xmlNamespace in xmlNamespaces)
                {
                    xmlSerializerNamespaces.Add(xmlNamespace.Key, xmlNamespace.Value);
                }
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = IDENT_CHARS,
                NewLineChars = NEW_LINE_CHARS,
                NewLineHandling = NewLineHandling.Replace,
                OmitXmlDeclaration = !setXmlDeclaration,
            };

            StringBuilder stringBuilder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(stringBuilder))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, tSaveObject, xmlSerializerNamespaces);
            }

            string resultXml = RemoveNilElementsFromXmlString(stringBuilder.ToString(), xmlWriterSettings);

            stringBuilder.Clear();

            if(showXmlVersion)
                stringBuilder.AppendLine(XML_VERSION_STRING);

            stringBuilder.Append(resultXml);

            return stringBuilder.ToString();
        }
        private string RemoveNilElementsFromXmlString(string xmlString, XmlWriterSettings xmlWriterSettings)
        {
            XDocument doc = XDocument.Parse(xmlString);

            doc.Descendants()
                    .Where(x => (bool?)x.Attribute(XName.Get(NIL_KEY, XMLNS_XSI_VALUE)) == true)
                    .Remove();

            return doc.ToString();
        }

    }
}
