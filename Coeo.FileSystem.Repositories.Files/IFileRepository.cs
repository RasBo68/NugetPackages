using Coeo.FileSystem.Repositories.Files.Models;

namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileRepository
    {
        Task DeleteAllFilesAsync(string directory);
        Task DeleteFileAsync(string filePath);
        void Dispose();
        Task<List<string>> DownloadAllFilesAsync(string directory);
        Task<string> DownloadFileAsync(string filePath);
        Task<IEnumerable<string>> ListAllFilesAsync(string directory);
        Task UploadAllFilesAsync(List<FileContent> contentStrings, string directory);
        Task UploadFileAsync(string contentString, string filePath);
    }
}