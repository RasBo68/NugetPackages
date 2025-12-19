using Coeo.FileSystem.Repositories.Files.Extensions;
using Coeo.FileSystem.Repositories.Files.Models;
using Renci.SshNet;
using System.Text;

namespace Coeo.FileSystem.Repositories.Files
{

    public class FileRepositorySftp : IDisposable, IFileRepository
    {
        private readonly IPathHelper _pathHelper;
        private readonly IDirectoryHelper _directoryHelper;
        private readonly IFileHelper _fileHelper;
        private readonly SftpClient _client;
        protected const string SFTP_CONNECTION_ERROR = "Could not connect to SFTP server.";

        /*
           Stream: A stream is simply an abstraction for reading and writing data – independent of where the data is located.
            Memorystream: Reads from or writes in RAM memory
            Filestream: Reads from or writes in disk memory
         */

        public FileRepositorySftp(IPathHelper pathHelper, IDirectoryHelper directoryHelper, IFileHelper fileHelper, SftpClient client)
        {
            _pathHelper = pathHelper;
            _directoryHelper = directoryHelper;
            _fileHelper = fileHelper;
            _pathHelper = pathHelper;
            _client = client;
            _client.ConnectSftpClient();
        }

        public async Task UploadAllFilesAsync(List<FileContent> contentStrings, string remoteDirectory)
        {
            await _directoryHelper.CheckDirectoryAsync(remoteDirectory);

            foreach (var contentString in contentStrings)
            {
                string remoteFilePath = Path.Combine(remoteDirectory, contentString.FileName);
                _pathHelper.CheckPathString(remoteFilePath);
                await UploadFileAsync(contentString.Content, remoteFilePath);
            }
        }
        public async Task<List<string>> DownloadAllFilesAsync(string remoteDirectory)
        {
            await _directoryHelper.CheckDirectoryAsync(remoteDirectory);
            List<string> fileContentStrings = new List<string>();
            var files = await ListAllFilesAsync(remoteDirectory);

            foreach (var file in files.ToList())
            {
                var downloadedContent = await DownloadFileAsync(file); 
                fileContentStrings.Add(downloadedContent);
            }

            return fileContentStrings;
        }
        public async Task DeleteAllFilesAsync(string remoteDirectory)
        {
            _pathHelper.CheckPathString(remoteDirectory);
            await _directoryHelper.CheckDirectoryAsync(remoteDirectory);
            var files = await ListAllFilesAsync(remoteDirectory);

            foreach (var file in files.ToList())
                await DeleteFileAsync(file);
        }
        public async Task<IEnumerable<string>> ListAllFilesAsync(string remoteDirectory)
        {
            _pathHelper.CheckPathString(remoteDirectory);
            await _directoryHelper.CheckDirectoryAsync(remoteDirectory); 

            return await Task.Run(() =>
            {
                var files = _client.ListDirectory(remoteDirectory)
                    .Where(f => !f.IsDirectory && !f.Name.StartsWith("."))
                    .Select(f => f.FullName);
                return files;
            });
        }
        public async Task UploadFileAsync(string contentString, string remoteFilePath)
        {
            _pathHelper.CheckPathString(remoteFilePath);
            var contentBytes = System.Text.Encoding.UTF8.GetBytes(contentString); // convert string to byte array
            using var ms = new MemoryStream(contentBytes);  // create memory stream in ram memory from byte array
            await _client.UploadFileAsync(ms, remoteFilePath);
        }
        public async Task<string> DownloadFileAsync(string remoteFilePath)
        {
            _pathHelper.CheckPathString(remoteFilePath);
            await _fileHelper.CheckFileAsync(remoteFilePath);

            // using -> automatically disposes the object after use
            using var ms = new MemoryStream(); // Buffer in ram memory, which behaves like a file
            await _client.DownloadFileAsync(remoteFilePath, ms); // download file from sftp to memory stream
            ms.Position = 0;    // after download the cursor of memory stream is at the end, set position to 0 to read from the beginning
            using var sr = new StreamReader(ms, Encoding.UTF8);    // stream reader to read from memory stream
            var contentString = sr.ReadToEnd();     // conversion bytes 2 string
            return contentString;
        }
        public async Task DeleteFileAsync(string remoteFilePath)
        {
            _pathHelper.CheckPathString(remoteFilePath);
            await _fileHelper.CheckFileAsync(remoteFilePath);
            await _client.DeleteAsync(remoteFilePath);
        }
        public void Dispose()
        {
            if (_client.IsConnected)
                _client.Disconnect();
            _client.Dispose();
        }
    }
}
