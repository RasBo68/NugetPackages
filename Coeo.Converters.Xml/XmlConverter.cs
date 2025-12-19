using Coeo.Converters.Xml.Extensions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Coeo.Converters.Xml
{
    public class XmlConverter : IXmlConverter
    {
        private const string IDENT_CHARS = "\t";
        private const string NEW_LINE_CHARS = "\n";


        public async Task<TLoadObject> ConvertXmlString2Object<TLoadObject>(string filePath)
        {
            filePath.CheckFilePathString();

            return await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TLoadObject));
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    return (TLoadObject)xmlSerializer.Deserialize(streamReader)! ?? Activator.CreateInstance<TLoadObject>();
                }
            });
        }
        public async Task<string> ConvertObject2XmlString<TSaveObject>(string filePath, TSaveObject tSaveObject)
        {
            filePath.CheckFilePathString();

            string xmlContent = string.Empty;

            await Task.Run(() =>
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
