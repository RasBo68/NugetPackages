
namespace Coeo.FileSystem.Repositories.Files
{
    public interface IDirectoryHelper
    {
        Task CheckDirectoryAsync(string directory);
        void CreateDirectoryAsync(string directory);
    }
}