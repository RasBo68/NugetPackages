
namespace Coeo.FileSystem.Repositories.Files
{
    public interface IFileHelper
    {
        Task CheckFileAsync(string filePath, CancellationToken? cancellationToken = null);
    }
}