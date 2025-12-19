using Coeo.FileSystem.Repositories.Files.Extensions;
using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelperSftp : FileHelperBase, IFileHelper
    {
        private readonly SftpClient _client;

        public FileHelperSftp(SftpClient client)
        {
            _client = client;
            _client.ConnectSftpClient();
        }
        public async Task CheckFileAsync(string remoteFilePath)
        {
            var fileExists = await _client.ExistsAsync(remoteFilePath);
            if (!fileExists)
                throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, remoteFilePath));
        }
    }
}
