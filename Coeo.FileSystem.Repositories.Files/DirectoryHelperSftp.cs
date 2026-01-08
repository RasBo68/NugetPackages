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

        }
        public async Task CheckDirectoryAsync(string remoteDirectory, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                var attributes = await _client.GetAttributesAsync(remoteDirectory, cancellationToken ?? CancellationToken.None);
                if (!attributes.IsDirectory)
                    throw new InvalidOperationException(string.Format(DIRECTORY_DOES_NOT_EXIST_ERROR, remoteDirectory));

                return Task.CompletedTask;
            });
        }
        public async Task CreateDirectoryAsync(string directory, CancellationToken? cancellationToken = null)
        {
            await ExecuteWithHandling(async () =>
            {
                _pathHelper.CheckPathString(directory);
                var dirName = _pathHelper.GetDirectoryName(directory);
                var dirPath = _pathHelper.GetFileNameWithoutExtension(directory);
                directory = _pathHelper.CombinePaths(dirName, dirPath);
                var token = cancellationToken ?? CancellationToken.None;
                if (!await _client.ExistsAsync(directory, token))
                    await _client.CreateDirectoryAsync(directory, token);

                return Task.CompletedTask;
            });


        }
    }
}
