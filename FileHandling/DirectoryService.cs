namespace FileHandling
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IPathHelper _pathHelper;

        public DirectoryService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
        public async Task CreateDirectoryAsync(string directory)
        {
            await Task.Run(() =>
            {
                Directory.CreateDirectory(directory);
            });
        }
        public async Task<List<string>> GetFilenames(string directory)
        {
            var filenames = new List<string>();

            // EnumerateFiles ist synchron und lädt immer wieder einen Dateipfad in den Arbeitsspeicher und würde den Thread bei vielen Dateien blockieren
            foreach (var filePath in Directory.EnumerateFiles(directory))
            {
                await Task.Yield(); // Sorgt stets für eine kleine Unterbrechung (Pause) der Schleife, damit der Thread responsiv bleibt
                filenames.Add(Path.GetFileName(filePath));
            }

            return filenames;
        }
        public async Task<List<string>> GetFilenamesWithSpecificExtensionAsync(string directory, string fileExtension)
        {
            List<string> filenames = await GetFilenames(directory);

            return filenames
                    .Where(fn => _pathHelper.GetFileExtension(fn) == fileExtension)
                    .ToList();
        }
    }
}
