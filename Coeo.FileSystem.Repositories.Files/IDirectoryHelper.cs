
namespace Coeo.FileSystem.Repositories.Files
{
    public interface IDirectoryHelper
    {
        Task CheckDirectoryAsync(string directory, CancellationToken? cancellationToken);
        Task CreateDirectoryAsync(string directory, CancellationToken? cancellationToken);
    }
}