using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelperSftp : FileHelperBase, IFileHelper
    {
        private readonly SftpClient _client;

        public FileHelperSftp(SftpClient client)
        {
            _client = client;
            _client.Connect();
            if (!_client.IsConnected)
                throw new InvalidOperationException(SFTP_CONNECTION_ERROR);
        }
        public async Task CheckFileAsync(string remoteFilePath)
        {
            if (!await _client.ExistsAsync(remoteFilePath))
                throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, remoteFilePath));
        }
    }
}
