using Coeo.FileSystem.Repositories.Files.Models;

namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileRepository
    {
        Task ConnectClient(CancellationToken? cancellationToken = null);
        Task DeleteFileAsync(string filePath, CancellationToken? cancellationToken = null);
        void Dispose();
        Task<FileObject> DownloadFileAsync(string filePath, CancellationToken? cancellationToken = null);
        Task<IEnumerable<string>> ListAllFilesAsync(string directory, CancellationToken? cancellationToken = null);
        Task UploadFileAsync(string contentString, string filePath, CancellationToken? cancellationToken = null);
    }
}