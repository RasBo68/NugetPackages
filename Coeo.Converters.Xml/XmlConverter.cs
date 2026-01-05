using Coeo.Converters.Xml.Extensions;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Coeo.Converters.Xml
{
    public class XmlConverter : IXmlConverter
    {
        private const string IDENT_CHARS = "\t";
        private const string NEW_LINE_CHARS = "\n";


        public TLoadObject ConvertXmlStringToObject<TLoadObject>(string xmlContentString)
        {
            using MemoryStream stream = new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes(xmlContentString));
            var serializer = new XmlSerializer(typeof(TLoadObject));
            TLoadObject? val = (TLoadObject?)serializer.Deserialize(stream);
            return (val != null) ? val : Activator.CreateInstance<TLoadObject>();
        }
        public string ConvertObjectToXmlString<TSaveObject>(TSaveObject tSaveObject)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TSaveObject));
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = IDENT_CHARS,
                NewLineChars = NEW_LINE_CHARS,
                NewLineHandling = NewLineHandling.Replace,
                OmitXmlDeclaration = true,
            };

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(stringBuilder);
            using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
            xmlSerializer.Serialize(xmlWriter, tSaveObject, xmlSerializerNamespaces);

            return stringBuilder.ToString();
        }
    }
}
