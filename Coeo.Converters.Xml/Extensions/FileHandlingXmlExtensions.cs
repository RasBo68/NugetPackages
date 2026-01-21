namespace Coeo.Converters.Xml.Extensions
{
    internal static class FileHandlingExtensions
    {
        private const string EMPTY_STRING_ERROR = "filePath is empty or just whitespace.";
        internal static void CheckFilePathString(this string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new InvalidOperationException(EMPTY_STRING_ERROR);
        }
    }
}
