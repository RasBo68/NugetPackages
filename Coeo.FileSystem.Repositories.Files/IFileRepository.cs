using Coeo.FileSystem.Repositories.Files.Models;
using System.Text;

namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileRepository
    {
        Task ConnectClient(CancellationToken? cancellationToken = null);
        Task DeleteFileAsync(string filePath, CancellationToken? cancellationToken = null);
        void Dispose();
        Task<FileObject> DownloadFileAsync(string filePath, Encoding encoding, CancellationToken? cancellationToken = null);
        Task<IEnumerable<string>> ListAllFilesAsync(string directory, CancellationToken? cancellationToken = null);
        Task UploadFileAsync(FileObject fileObject, Encoding encoding, CancellationToken? cancellationToken = null);
    }
}