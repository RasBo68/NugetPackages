
using Coeo.FileSystem.Repositories.Files.Models;
using System.Text;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileRepository : FileRepositoryBase, IFileRepository
    {
        private readonly IPathHelper _pathHelper;
        private readonly IDirectoryHelper _directoryHelper;
        private readonly IFileHelper _fileHelper;

        public FileRepository(IPathHelper pathHelper, IDirectoryHelper directoryHelper, IFileHelper fileHelper)
        {
            _pathHelper = pathHelper;
            _directoryHelper = directoryHelper;
            _fileHelper = fileHelper;
        }
        public Task ConnectClient(CancellationToken? cancellationToken = null)
        {
            // No connection necessary
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<string>> ListAllFilesAsync(string directory, CancellationToken? cancellationToken = null)
        {
            return await ExecuteWithHandling(async () =>
            {
                cancellationToken?.ThrowIfCancellationRequested();

                _pathHelper.CheckPathString(directory);
                await _directoryHelper.CheckDirectoryAsync(directory, cancellationToken);
                var files = Directory.GetFiles(directory).AsEnumerable(); // no asynchronous version available, cause operation is fast
                return files;
            });

        }
        public async Task UploadFileAsync(string contentString, string filePath, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                cancellationToken?.ThrowIfCancellationRequested();

                _pathHelper.CheckPathString(filePath);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                await File.WriteAllTextAsync(filePath, contentString, cancellationToken ?? CancellationToken.None);
            });
        }
        public async Task<FileObject> DownloadFileAsync(string filePath, CancellationToken? cancellationToken = null)
        {
            return await ExecuteWithHandling(async () =>
            {
                cancellationToken?.ThrowIfCancellationRequested();

                _pathHelper.CheckPathString(filePath);
                await _fileHelper.CheckFileAsync(filePath, cancellationToken);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                var contentString = await File.ReadAllTextAsync(filePath, cancellationToken ?? CancellationToken.None);
                return new FileObject { Path = filePath, Content = contentString } ;
            });
        }
        public async Task DeleteFileAsync(string filePath, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                cancellationToken?.ThrowIfCancellationRequested();

                _pathHelper.CheckPathString(filePath);
                await _fileHelper.CheckFileAsync(filePath, cancellationToken);
                File.Delete(filePath); // no asynchronous version available, cause operation is fast
            });
        }
        public void Dispose()
        {
            // No unmanaged resources to dispose
        }
    }
}
