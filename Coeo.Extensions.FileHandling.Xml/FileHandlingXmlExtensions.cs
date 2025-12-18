namespace Coeo.Extensions.FileHandling.Xml
{
    internal static class FileHandlingExtensions
    {
        internal static void CheckFilePathString(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
                throw new InvalidOperationException("filePath is empty or just whitespace.");
        }
    }
}
