using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Coeo.Extensions.FileHandling.Xml
{
    public class XmlService : IXmlService
    {
        private readonly string identChars = "\t";
        private readonly string newLineChars = "\n";

        public async Task XmlFromFile<TLoadObject>(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TLoadObject));
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    return (TLoadObject)xmlSerializer.Deserialize(streamReader)! ?? Activator.CreateInstance<TLoadObject>();
                }
            });
        }
        public async Task Xml2FileAsync<TSaveObject>(string filePath, TSaveObject tSaveObject)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(tSaveObject);

            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TSaveObject));
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty); // Unterdrückt die Namespaces
                XmlWriterSettings XmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = identChars,  // Verwenden Sie einen Tab für Einrückungen
                    NewLineChars = newLineChars,  // Stellen Sie sicher, dass Zeilenumbrüche konsistent sind (optional)
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = true,
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
                {
                    xmlSerializer.Serialize(xmlWriter, tSaveObject, xmlSerializerNamespaces);
                }
            });
        }
        public async Task<string> Xml2String<TSaveObject>(string filePath, TSaveObject tSaveObject)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(tSaveObject);

            string xmlContent = string.Empty;

            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TSaveObject));
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty); // Unterdrücke die Namespaces
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",  // Tab für Einrückungen
                    NewLineChars = "\n",  // Zeilenumbrüche konsistent
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = true,  // XML-Deklaration weglassen
                };

                StringBuilder stringBuilder = new StringBuilder();
                using (StringWriter stringWriter = new StringWriter(stringBuilder))
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    {
                        xmlSerializer.Serialize(xmlWriter, tSaveObject, xmlSerializerNamespaces);
                    }
                }

                string xmlContent = stringBuilder.ToString();
            });

            return xmlContent;
        }
    }
}
