namespace Coeo.Extensions.FileHandling.Base
{
    public class FileService : IFileService
    {
        private readonly string _argumentOutOfRangeExceptionMessage = "An attempt was made to read line {0} from file {1}.";

        private readonly IDirectoryService _directoryService;
        private readonly IPathHelper _pathHelper;

        public FileService(IDirectoryService directoryService, IPathHelper pathHelper)
        {
            _directoryService = directoryService;
            _pathHelper = pathHelper;
        }

        public async Task CreateNewFileAsync(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            var directory = _pathHelper.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                await _directoryService.CreateDirectoryAsync(directory);
            }

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1, useAsync: true);
            await fs.DisposeAsync();
        }
        public async Task AppendLinesToFileAsync(string filePath, List<string> contentToWrite)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(contentToWrite);

            await File.AppendAllLinesAsync(filePath, contentToWrite);
        }
        public async Task<string> ReadAllLinesOfFileAsync(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            return await File.ReadAllTextAsync(filePath) ?? string.Empty;
        }
        public async Task<string> ReadLineOfFileAsync(string filePath, int lineToRead)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

           if(lineToRead < 0)
           {
                throw new ArgumentOutOfRangeException(string.Format(_argumentOutOfRangeExceptionMessage, lineToRead, filePath));
           }

            using var reader = new StreamReader(filePath);
            for (int lineIndex = 0; lineIndex <= lineToRead; lineIndex++)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format(_argumentOutOfRangeExceptionMessage, lineToRead, filePath));
                }

                if (lineIndex == lineToRead)
                    return line;
            }

            return string.Empty;
        }
        public async Task DeleteFileAsync(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            await Task.Run(() =>
            {
                File.Delete(filePath);
            });
        }
    }

}

