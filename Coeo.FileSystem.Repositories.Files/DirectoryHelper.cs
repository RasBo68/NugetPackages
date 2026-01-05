namespace Coeo.FileSystem.Repositories.Files
{
    public class DirectoryHelper : FileRepositoryBase, IDirectoryHelper
    {
        private readonly IPathHelper _pathHelper;
        public DirectoryHelper(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
        public async Task CheckDirectoryAsync(string directory)
        {
            await ExecuteWithHandling(() =>
            {
                var fileExists = Directory.Exists(directory); // no asynchronous version available, cause operation is fast
                if (!fileExists) 
                    throw new InvalidOperationException(string.Format(DIRECTORY_DOES_NOT_EXIST_ERROR, directory));
                return Task.CompletedTask;
            });
        }
        public async Task CreateDirectoryAsync(string directory)
        {
            await ExecuteWithHandling(() =>
            {
                _pathHelper.CheckPathString(directory);
                var dirName = _pathHelper.GetDirectoryName(directory);
                var dirPath = _pathHelper.GetFileNameWithoutExtension(directory);
                directory = _pathHelper.CombinePaths(dirName, dirPath);
                Directory.CreateDirectory(directory); // no asynchronous version available, cause operation is fast
                return Task.CompletedTask;
            });
        }
    }
}
