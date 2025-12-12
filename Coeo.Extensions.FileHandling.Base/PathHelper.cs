
namespace Coeo.Extensions.FileHandling.Base
{
    public class PathHelper : IPathHelper
    {
        public bool ValidateFile(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return File.Exists(filePath);
        }
        public bool ValidateDirectory(string directory)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(directory);

            return Directory.Exists(directory);
        }

        public string CombinePaths(string string1, string string2)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(string1);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(string2);

            return Path.Combine(string1, string2);
        }
        public string GetDirectoryName(string? filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return Path.GetDirectoryName(filePath) ?? string.Empty;
        }
        public string GetFileName(string? filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return Path.GetFileName(filePath) ?? string.Empty;
        }
        public string GetFileNameWithoutExtension(string? filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
        }
        public string GetFileExtension(string? filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return Path.GetExtension(filePath) ?? string.Empty;
        }
    }
}
