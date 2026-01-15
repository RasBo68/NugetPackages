
using Coeo.FileSystem.Repositories.Files.Models;
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

        private static string SFTP_CONNECTION_ERROR = "Could not connect to SFTP server. Please check the sftp credentials.";
        private static string SFTP_CONNECTION_EXCEPTION = "Unexpected exception appeared during connection to SFTP server.";
        private static string SFTP_DISCONNECTION_EXCEPTION = "Could not disconnect from SFTP server.";
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
        }

        public async Task ConnectClient(CancellationToken? cancellationToken = null)
        {
            try
            {
                if (!_client.IsConnected)
                {
                    await _client.ConnectAsync(cancellationToken ?? CancellationToken.None);
                    if (!_client.IsConnected)
                        throw new InvalidOperationException(SFTP_CONNECTION_ERROR);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(SFTP_CONNECTION_EXCEPTION, ex);
            }
        }
        public async Task<IEnumerable<string>> ListAllFilesAsync(string remoteDirectory, CancellationToken? cancellationToken = null)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteDirectory);
                await _directoryHelper.CheckDirectoryAsync(remoteDirectory, cancellationToken ?? CancellationToken.None);
                return await Task.Run(() =>
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    var files = _client.ListDirectory(remoteDirectory)
                        .Where(f => !f.IsDirectory && !f.Name.StartsWith("."))
                        .Select(f => f.FullName);
                    return files;
                });
            });
        }
        public async Task UploadFileAsync(FileObject remoteFileObject, Encoding encoding, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFileObject.Path);
                // convert string to byte array
                var contentBytes = encoding.GetBytes(remoteFileObject.Content); 
                using var ms = new MemoryStream(contentBytes);  // create memory stream in ram memory from byte array
                await _client.UploadFileAsync(ms, remoteFileObject.Path, cancellationToken ?? CancellationToken.None);
                return Task.CompletedTask;
            });
        }
        public async Task<FileObject> DownloadFileAsync(string remoteFilePath, Encoding encoding, CancellationToken? cancellationToken = null)
        {
            return await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFilePath);
                var token = cancellationToken ?? CancellationToken.None;
                await _fileHelper.CheckFileAsync(remoteFilePath, token);
                // using -> automatically disposes the object after use
                using var ms = new MemoryStream(); // Buffer in ram memory, which behaves like a file
                await _client.DownloadFileAsync(remoteFilePath, ms, token); // download file from sftp to memory stream
                ms.Position = 0;    // after download the cursor of memory stream is at the end, set position to 0 to read from the beginning
                // stream reader to read from memory stream
                // SFTP servers often use ISO-8859-1 encoding, which is due to the Linux system.
                using var sr = new StreamReader(ms, encoding);   
                var contentString = sr.ReadToEnd();     // conversion bytes 2 string
                return new FileObject {Path = remoteFilePath,  Content = contentString };
            });
        }
        public async Task DeleteFileAsync(string remoteFilePath, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(remoteFilePath);
                var token = cancellationToken ?? CancellationToken.None;
                await _fileHelper.CheckFileAsync(remoteFilePath, token);
                await _client.DeleteAsync(remoteFilePath, token);
                return Task.CompletedTask;
            });
        }
        public void Dispose()
        {
            if(!_disposed)
            {
                DisconnectSftpClient();
                _disposed = true;
            }
        }

        private void DisconnectSftpClient()
        {
            try
            {
                if (_client.IsConnected)
                    _client.Disconnect();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(SFTP_DISCONNECTION_EXCEPTION, ex);
            }
        }
    }
}
