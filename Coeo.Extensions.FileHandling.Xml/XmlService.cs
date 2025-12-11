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

        public async Task LoadXmlFileAsync<TLoadObject>(string filePath)
        {
            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TLoadObject));
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    return (TLoadObject)xmlSerializer.Deserialize(streamReader)! ?? Activator.CreateInstance<TLoadObject>();
                }
            });
        }



        public async Task SaveXmlFileAsync<TSaveObject>(string filePath, TSaveObject tSaveObject, List<string> linesToDelete)
        {
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
                    OmitXmlDeclaration = true
                };
                using (XmlWriter XmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
                {
                    xmlSerializer.Serialize(XmlWriter, tSaveObject, xmlSerializerNamespaces);
                }
            });
        }
    }
}
