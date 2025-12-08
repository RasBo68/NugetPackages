namespace FileHandling
{
    public interface IPathHelper
    {
        string CombinePaths(string string1, string string2);
        string GetFileExtension(string? filePath);
        string GetFileName(string? filePath);
        string GetFileNameWithoutExtension(string? filePath);
        string GetDirectoryName(string? filePath);
        bool ValidatePath(string filePath);
    }
}