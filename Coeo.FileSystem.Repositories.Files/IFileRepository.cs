using Coeo.FileSystem.Repositories.Files.Models;

namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileRepository
    {
        Task DeleteFileAsync(string filePath);
        void Dispose();
        Task<string> DownloadFileAsync(string filePath);
        Task<IEnumerable<string>> ListAllFilesAsync(string directory);
        Task UploadFileAsync(string contentString, string filePath);
    }
}