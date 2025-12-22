using Org.BouncyCastle.Asn1;

namespace Coeo.FileSystem.Repositories.Files
{
    public class FileHelper : FileRepositoryBase, IFileHelper
    {
        public async Task CheckFileAsync(string filePath)
        {
            // This method does not require a bool return value, as its purpose is to validate the existence of the file with exception hanndling
            await ExecuteWithHandling(() =>
            {
                var fileExists = File.Exists(filePath);
                if (!fileExists)
                    throw new InvalidOperationException(string.Format(FILE_DOES_NOT_EXIST_ERROR, filePath));

                return Task.CompletedTask; // pretends to be asynchronous
            });
        }
    }
}
