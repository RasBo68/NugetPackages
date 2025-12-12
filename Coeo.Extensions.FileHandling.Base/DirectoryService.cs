using System.IO;
using System.Text.RegularExpressions;

namespace Coeo.Extensions.FileHandling.Base
{
    public class DirectoryService : IDirectoryService
    {
        private readonly string _isFilePattern = @"^[a-zA-Z]:\\(?:[\w-]+\\)*[\w-]+\.[a-zA-Z0-9]+$";
        private readonly IPathHelper _pathHelper;

        public DirectoryService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
        public async Task CreateDirectoryAsync(string directory)
        {
            directory.CheckDirectoryString();

            string dir = directory;

            if (IsPathToFile(directory))
            {
                var dirName = _pathHelper.GetDirectoryName(dir);
                var dirPath = _pathHelper.GetFileNameWithoutExtension(dir);
                dir = _pathHelper.CombinePaths(dirName, dirPath);
            }


            await Task.Run(() =>
            {
                Directory.CreateDirectory(dir);
            });
        }
        public async Task<List<string>> GetFilenames(string directory)
        {
            directory.CheckDirectoryString();

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
            directory.CheckDirectoryString();

            if (string.IsNullOrEmpty(fileExtension) || string.IsNullOrWhiteSpace(fileExtension))
                throw new InvalidOperationException("fileExtension is empty or just whitespace.");

            List<string> filenames = await GetFilenames(directory);

            return filenames
                    .Where(fn => _pathHelper.GetFileExtension(fn).ToLower() == fileExtension.ToLower())
                    .ToList();
        }
        private bool IsPathToFile(string path)
        {
            var i = Regex.IsMatch(path, _isFilePattern);
            return Regex.IsMatch(path, _isFilePattern);
        }
   
    }
}
