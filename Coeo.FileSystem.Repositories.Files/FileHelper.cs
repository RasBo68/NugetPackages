namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelper : FileHelperBase, IFileHelper
    {
        public Task CheckFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) // no asynchronous version available, cause operation is fast
                throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, filePath));

            return Task.CompletedTask;
        }
    }
}
