using Coeo.FileSystem.Repositories.Files.Extensions;
using Renci.SshNet;
using System.Text;

namespace Coeo.FileSystem.Repositories.Files
{

    public class FileRepositorySftp : FileRepositoryBase, IDisposable, IFileRepository
    {
        private readonly IPathHelper _pathHelper;
        private readonly IDirectoryHelper _directoryHelper;
        private readonly IFileHelper _fileHelper;
        private readonly SftpClient _client;
        protected const string SFTP_CONNECTION_ERROR = "Could not connect to SFTP server.";
        private bool _disposed = false;

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

        public async Task<IEnumerable<string>> ListAllFilesAsync(string remoteDirectory)
        {
            return await ExecuteWithHandling(async () =>
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
            });
        }
        public async Task UploadFileAsync(string contentString, string remoteFilePath)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFilePath);
                var contentBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(contentString); // convert string to byte array
                using var ms = new MemoryStream(contentBytes);  // create memory stream in ram memory from byte array
                await _client.UploadFileAsync(ms, remoteFilePath);
                return Task.CompletedTask;
            });
        }
        public async Task<string> DownloadFileAsync(string remoteFilePath)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFilePath);
                await _fileHelper.CheckFileAsync(remoteFilePath);
                // using -> automatically disposes the object after use
                using var ms = new MemoryStream(); // Buffer in ram memory, which behaves like a file
                await _client.DownloadFileAsync(remoteFilePath, ms); // download file from sftp to memory stream
                ms.Position = 0;    // after download the cursor of memory stream is at the end, set position to 0 to read from the beginning
                using var sr = new StreamReader(ms, Encoding.GetEncoding("ISO-8859-1"));    // stream reader to read from memory stream
                var contentString = sr.ReadToEnd();     // conversion bytes 2 string
                return contentString;
            });
        }
        public async Task DeleteFileAsync(string remoteFilePath)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFilePath);
                await _fileHelper.CheckFileAsync(remoteFilePath);
                await _client.DeleteAsync(remoteFilePath);
                return Task.CompletedTask;
            });
        }
        public void Dispose()
        {
            if(!_disposed)
            {
                _client.DisconnectSftpClient();
                _disposed = true;
            }
        }
    }
}
