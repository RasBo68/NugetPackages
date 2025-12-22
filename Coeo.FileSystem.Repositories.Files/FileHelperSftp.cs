using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelperSftp : FileRepositoryBase, IFileHelper
    {
        private readonly SftpClient _client;

        public FileHelperSftp(SftpClient client)
        {
            _client = client;
        }
        public async Task CheckFileAsync(string remoteFilePath)
        {
            await ExecuteWithHandlingAsync(async() =>
            {
                var fileExists = await _client.ExistsAsync(remoteFilePath);
                if (!fileExists)
                    throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, remoteFilePath));

                return Task.CompletedTask;
            }); 
        }
    }
}
