
using System.Text;
using System.IO;
using Coeo.FileSystem.Repositories.Files.Models;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileRepository : IFileRepository
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

        public async Task UploadAllFilesAsync(List<FileContent> contentStrings, string directory)
        {
            await _directoryHelper.CheckDirectoryAsync(directory);

            foreach (var contentString in contentStrings)
            {
                string remoteFilePath = Path.Combine(directory, contentString.FileName);
                _pathHelper.CheckPathString(remoteFilePath);
                await UploadFileAsync(contentString.Content, remoteFilePath);
            }
        }
        public async Task<List<string>> DownloadAllFilesAsync(string directory)
        {
            await _directoryHelper.CheckDirectoryAsync(directory);
            List<string> fileContentStrings = new List<string>();
            var files = await ListAllFilesAsync(directory);

            foreach (var file in files.ToList())
            {
                var downloadedContent = await DownloadFileAsync(file);
                fileContentStrings.Add(downloadedContent);
            }

            return fileContentStrings;
        }
        public async Task DeleteAllFilesAsync(string directory)
        {
            _pathHelper.CheckPathString(directory);
            await _directoryHelper.CheckDirectoryAsync(directory);
            var files = await ListAllFilesAsync(directory);

            foreach (var filePath in files)
                await DeleteFileAsync(filePath);
        }
        public async Task<IEnumerable<string>> ListAllFilesAsync(string directory)
        {
            _pathHelper.CheckPathString(directory);
            await _directoryHelper.CheckDirectoryAsync(directory);
            var files = Directory.GetFiles(directory).AsEnumerable(); // no asynchronous version available, cause operation is fast
            return files;
        }
        public async Task UploadFileAsync(string contentString, string filePath)
        {
            _pathHelper.CheckPathString(filePath);
            await _fileHelper.CheckFileAsync(filePath);
            await File.WriteAllTextAsync(filePath, contentString, Encoding.UTF8);
        }
        public async Task<string> DownloadFileAsync(string filePath)
        {
            _pathHelper.CheckPathString(filePath);
            await _fileHelper.CheckFileAsync(filePath);
            return await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        }
        public Task DeleteFileAsync(string filePath)
        {
            _pathHelper.CheckPathString(filePath);
            _fileHelper.CheckFileAsync(filePath);
            File.Delete(filePath); // no asynchronous version available, cause operation is fast
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            // No unmanaged resources to dispose
        }
    }
}
