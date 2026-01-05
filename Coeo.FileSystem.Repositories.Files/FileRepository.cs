
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

        public async Task<IEnumerable<string>> ListAllFilesAsync(string directory)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(directory);
                await _directoryHelper.CheckDirectoryAsync(directory);
                var files = Directory.GetFiles(directory).AsEnumerable(); // no asynchronous version available, cause operation is fast
                return files;
            });

        }
        public async Task UploadFileAsync(string contentString, string filePath)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(filePath);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                await File.WriteAllTextAsync(filePath, contentString);
            });
        }
        public async Task<string> DownloadFileAsync(string filePath)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(filePath);
                await _fileHelper.CheckFileAsync(filePath);
                // In the windows file system, the default encoding is UTF-16 LE (Little Endian).
                return await File.ReadAllTextAsync(filePath);
            });
        }
        public async Task DeleteFileAsync(string filePath)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(filePath);
                await _fileHelper.CheckFileAsync(filePath);
                File.Delete(filePath); // no asynchronous version available, cause operation is fast
            });
        }
        public void Dispose()
        {
            // No unmanaged resources to dispose
        }
    }
}
