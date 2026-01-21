
using Coeo.FileSystem.Repositories.Database.Exceptions;
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
        private const string DESERIALIZATION_EXCEPTION = "Deserialization of the xmlContentString \n {0} \n failed.";
        private const string SERIALIZATION_EXCEPTION = "serialization of the object \n {0} \n failed.";
        private const string JSON_STRING_EMPTY_OR_WHITESPACE = "The input xml string content is either null or empty.";

        public TLoadObject ConvertXmlStringToObject<TLoadObject>(string xmlContentString)
        {
            if (string.IsNullOrWhiteSpace(xmlContentString))
                throw new ArgumentException(JSON_STRING_EMPTY_OR_WHITESPACE);

            try
            {
                using (StringReader stringReader = new StringReader(xmlContentString))
                {
                    var serializer = new XmlSerializer(typeof(TLoadObject));
                    var serializedObject = (TLoadObject)serializer.Deserialize(stringReader)!;
                    return serializedObject;
                }
            }
            catch (Exception ex)
            {
                throw new XmlSerializationException(
                    string.Format(DESERIALIZATION_EXCEPTION, xmlContentString),
                    ex);
            }
        }
        public string ConvertObjectToXmlString<TSaveObject>(
            TSaveObject tSaveObject,
            Dictionary<string, string>? xmlNamespaces = null,
            bool setXmlDeclaration = false, 
            bool showXmlVersion = true)
        {
            string xmlContentString = string.Empty;

            try
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

                if (showXmlVersion)
                    stringBuilder.AppendLine(XML_VERSION_STRING);

                stringBuilder.Append(resultXml);

                xmlContentString = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new XmlDeserializationException(string.Format(SERIALIZATION_EXCEPTION, tSaveObject?.ToString()), ex);
            }

            return xmlContentString;
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
