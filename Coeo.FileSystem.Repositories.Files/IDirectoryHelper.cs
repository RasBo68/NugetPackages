
namespace Coeo.FileSystem.Repositories.Files
{
    public interface IDirectoryHelper
    {
        Task CheckDirectoryAsync(string directory);
        Task CreateDirectoryAsync(string directory);
    }
}