using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files.Extensions
{
    internal static class SftpClientExtension
    {
        private static string SFTP_CONNECTION_ERROR = "Could not connect to SFTP server.";

        internal static void ConnectSftpClient(this SftpClient sftpClient)
        {
            if (!sftpClient.IsConnected)
                sftpClient.Connect();

            if (!sftpClient.IsConnected)
                throw new InvalidOperationException(SFTP_CONNECTION_ERROR);
        }

        internal static void DisconnectSftpClient(this SftpClient sftpClient)
        {
            if (sftpClient.IsConnected)
                sftpClient.Disconnect();
        }
    }
}
