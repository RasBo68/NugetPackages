
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

        public async Task<IEnumerable<string>> ListAllFilesAsync(string directory, CancellationToken? cancellationToken)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(directory);
                await _directoryHelper.CheckDirectoryAsync(directory, cancellationToken);
                var files = Directory.GetFiles(directory).AsEnumerable(); // no asynchronous version available, cause operation is fast
                return files;
            });

        }
        public async Task UploadFileAsync(string contentString, string filePath, CancellationToken? cancellationToken)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(filePath);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                await File.WriteAllTextAsync(filePath, contentString);
            });
        }
        public async Task<string> DownloadFileAsync(string filePath, CancellationToken? cancellationToken)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(filePath);
                await _fileHelper.CheckFileAsync(filePath, cancellationToken);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                return await File.ReadAllTextAsync(filePath);
            });
        }
        public async Task DeleteFileAsync(string filePath, CancellationToken? cancellationToken)
        {
            await ExecuteWithHandling(async () =>
            {
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
