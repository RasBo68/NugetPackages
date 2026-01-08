using Coeo.FileSystem.Repositories.Files.Models;

namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileRepository
    {
        Task DeleteFileAsync(string filePath, CancellationToken? cancellationToken);
        void Dispose();
        Task<string> DownloadFileAsync(string filePath, CancellationToken? cancellationToken);
        Task<IEnumerable<string>> ListAllFilesAsync(string directory, CancellationToken? cancellationToken);
        Task UploadFileAsync(string contentString, string filePath, CancellationToken? cancellationToken);
    }
}