using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files
{
    public class DirectoryHelperSftp : DirectoryHelperBase, IDirectoryHelper
    {
        private readonly SftpClient _client;
        private readonly IPathHelper _pathHelper = new PathHelper();

        public DirectoryHelperSftp(SftpClient client, IPathHelper pathHelper)
        {
            _client = client;
            _client.Connect();
            if (!_client.IsConnected)
                throw new InvalidOperationException(SFTP_CONNECTION_ERROR);
        }
        public async Task CheckDirectoryAsync(string remoteDirectory)
        {
            var attributes = await _client.GetAttributesAsync(remoteDirectory, CancellationToken.None);
            if (!attributes.IsDirectory)
                throw new InvalidOperationException(string.Format(Directory_DOES_NOT_EXIST_ERROR, remoteDirectory));
        }
        public void CreateDirectoryAsync(string directory)
        {
            _pathHelper.CheckPathString(directory);
            var dirName = _pathHelper.GetDirectoryName(directory);
            var dirPath = _pathHelper.GetFileNameWithoutExtension(directory);
            directory = _pathHelper.CombinePaths(dirName, dirPath);
            Directory.CreateDirectory(directory); // no asynchronous version available, cause operation is fast
        }
    }
}
