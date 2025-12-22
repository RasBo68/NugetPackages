using Coeo.FileSystem.Repositories.Files.Extensions;
using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files
{
    public class DirectoryHelperSftp : FileRepositoryBase, IDirectoryHelper
    {
        private readonly SftpClient _client;
        private readonly IPathHelper _pathHelper = new PathHelper();

        public DirectoryHelperSftp(SftpClient client, IPathHelper pathHelper)
        {
            _client = client;
            _client.ConnectSftpClient();
        }
        public async Task CheckDirectoryAsync(string remoteDirectory)
        {
            await ExecuteWithHandling(async () =>
            {
                var attributes = await _client.GetAttributesAsync(remoteDirectory, CancellationToken.None);
                if (!attributes.IsDirectory)
                    throw new InvalidOperationException(string.Format(DIRECTORY_DOES_NOT_EXIST_ERROR, remoteDirectory));

                return Task.CompletedTask;
            });
        }
        public async Task CreateDirectoryAsync(string directory)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(directory);
                var dirName = _pathHelper.GetDirectoryName(directory);
                var dirPath = _pathHelper.GetFileNameWithoutExtension(directory);
                directory = _pathHelper.CombinePaths(dirName, dirPath);
                if (!_client.Exists(directory))
                    await Task.Run(() => _client.CreateDirectory(directory));

                return Task.CompletedTask;
            });


        }
    }
}
