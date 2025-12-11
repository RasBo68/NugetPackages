namespace Coeo.Extensions.FileHandling.Base
{
    public class PathHelper : IPathHelper
    {
        public bool ValidateFile(string filePath)
        {
            return File.Exists(filePath);
        }
        public bool ValidateDirectory(string directory)
        {
            return Directory.Exists(directory);
        }

        public string CombinePaths(string string1, string string2)
        {
            return Path.Combine(string1, string2);
        }
        public string GetDirectoryName(string? filePath)
        {
            return Path.GetDirectoryName(filePath) ?? string.Empty;
        }
        public string GetFileName(string? filePath)
        {
            return Path.GetFileName(filePath) ?? string.Empty;
        }
        public string GetFileNameWithoutExtension(string? filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
        }
        public string GetFileExtension(string? filePath)
        {
            return Path.GetExtension(filePath) ?? string.Empty;
        }
    }
}
