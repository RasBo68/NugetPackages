using System.IO;

namespace Coeo.FileSystem.Repositories.Files
{
    public class PathHelper : IPathHelper
    {
        protected const string PATH_IS_EMPTY_ERROR = "{0} is empty or just whitespace.";

        public string CombinePaths(string string1, string string2)
        {
            CheckPathString(string1);
            CheckPathString(string2);
            return Path.Combine(string1, string2);
        }
        public string GetDirectoryName(string filePath)
        {
            CheckPathString(filePath);
            return Path.GetDirectoryName(filePath) ?? string.Empty;
        }
        public string GetFileName(string filePath)
        {
            CheckPathString(filePath);
            return Path.GetFileName(filePath) ?? string.Empty;
        }
        public string GetFileNameWithoutExtension(string filePath)
        {
            CheckPathString(filePath);
            return Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
        }
        public string GetFileExtension(string filePath)
        {
            CheckPathString(filePath);
            return Path.GetExtension(filePath) ?? string.Empty;
        }
        public void CheckPathString(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException(string.Format(PATH_IS_EMPTY_ERROR, nameof(path)));
        }
    }
}
