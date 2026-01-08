using Renci.SshNet;
using System.Threading;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelperSftp : FileRepositoryBase, IFileHelper
    {
        private readonly SftpClient _client;

        public FileHelperSftp(SftpClient client)
        {
            _client = client;
        }
        public async Task CheckFileAsync(string remoteFilePath, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandlingAsync(async() =>
            {
                var fileExists = await _client.ExistsAsync(remoteFilePath, cancellationToken ?? CancellationToken.None);
                if (!fileExists)
                    throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, remoteFilePath));

                return Task.CompletedTask;
            }); 
        }
    }
}
