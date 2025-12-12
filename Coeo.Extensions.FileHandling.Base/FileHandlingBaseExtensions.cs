
namespace Coeo.Extensions.FileHandling.Base
{
    internal static class FileHandlingBaseExtensions
    {
        internal static void CheckDirectoryString(this string directory)
        {
            if (string.IsNullOrEmpty(directory) || string.IsNullOrWhiteSpace(directory))
                throw new InvalidOperationException("directory is empty or just whitespace.");
        }

        internal static void CheckFilePathString(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
                throw new InvalidOperationException("filePath is empty or just whitespace.");
        }
    }
}
