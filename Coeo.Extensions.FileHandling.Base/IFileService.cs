namespace Coeo.Extensions.FileHandling.Base
{
    public interface IFileService
    {
        Task AppendLinesToFileAsync(string filePath, List<string> contentToWrite);
        Task CreateNewFileAsync(string filePath);
        Task<string> ReadAllLinesOfFileAsync(string filePath);
        Task<string> ReadLineOfFileAsync(string filePath, int lineToRead);
        Task DeleteFileAsync(string filePath);
    }
}