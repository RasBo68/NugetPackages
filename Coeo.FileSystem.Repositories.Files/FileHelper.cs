namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelper : FileHelperBase, IFileHelper
    {
        public Task CheckFileAsync(string filePath)
        {
            var fileExists = File.Exists(filePath);
            if (!fileExists)
                throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, filePath));

            return Task.CompletedTask;
        }
    }
}
