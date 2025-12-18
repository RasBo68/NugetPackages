namespace Coeo.FileSystem.Repositories.Files
{
    public interface IPathHelper
    {
        void CheckPathString(string path);
        string CombinePaths(string string1, string string2);
        string GetDirectoryName(string filePath);
        string GetFileExtension(string filePath);
        string GetFileName(string filePath);
        string GetFileNameWithoutExtension(string filePath);
    }
}