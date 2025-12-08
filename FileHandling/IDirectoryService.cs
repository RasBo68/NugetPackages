
namespace FileHandling
{
    public interface IDirectoryService
    {
        Task CreateDirectoryAsync(string directory);
        Task<List<string>> GetFilenames(string directory);
        Task<List<string>> GetFilenamesWithSpecificExtensionAsync(string directory, string fileExtension);
    }
}