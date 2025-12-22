namespace Coeo.FileSystem.Repositories.Files
{
    public class DirectoryHelper : DirectoryHelperBase, IDirectoryHelper
    {
        private readonly IPathHelper _pathHelper;
        public DirectoryHelper(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
        public Task CheckDirectoryAsync(string directory)
        {
            var fileExists = Directory.Exists(directory);
            if (!fileExists) // no asynchronous version available, cause operation is fast
                throw new InvalidOperationException(string.Format(Directory_DOES_NOT_EXIST_ERROR, directory));

            return Task.CompletedTask;
        }
        public Task CreateDirectoryAsync(string directory)
        {
            _pathHelper.CheckPathString(directory);
            var dirName = _pathHelper.GetDirectoryName(directory);
            var dirPath = _pathHelper.GetFileNameWithoutExtension(directory);
            directory = _pathHelper.CombinePaths(dirName, dirPath);
            Directory.CreateDirectory(directory); // no asynchronous version available, cause operation is fast
            return Task.CompletedTask;
        }
    }
}
