
using System.IO;

namespace Coeo.Extensions.FileHandling.Base
{
    public class PathHelper : IPathHelper
    {
        public bool ValidateFile(string filePath)
        {
            filePath.CheckFilePathString();

            return File.Exists(filePath);
        }
        public bool ValidateDirectory(string directory)
        {
            directory.CheckFilePathString();

            return Directory.Exists(directory);
        }

        public string CombinePaths(string string1, string string2)
        {
            string1.CheckFilePathString();
            string2.CheckFilePathString();

            return Path.Combine(string1, string2);
        }
        public string GetDirectoryName(string filePath)
        {
            filePath.CheckFilePathString();

            return Path.GetDirectoryName(filePath) ?? string.Empty;
        }
        public string GetFileName(string filePath)
        {
            filePath.CheckFilePathString();

            return Path.GetFileName(filePath) ?? string.Empty;
        }
        public string GetFileNameWithoutExtension(string filePath)
        {
            filePath.CheckFilePathString();

            return Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
        }
        public string GetFileExtension(string filePath)
        {
            filePath.CheckFilePathString();

            return Path.GetExtension(filePath) ?? string.Empty;
        }
    }
}
