namespace Coeo.FileSystem.Repositories.Files.Models
{
    public class SftpOption
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 22;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
