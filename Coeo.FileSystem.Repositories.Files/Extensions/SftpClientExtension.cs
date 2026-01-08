using Renci.SshNet;

namespace Coeo.FileSystem.Repositories.Files.Extensions
{
    internal static class SftpClientExtension
    {
        private static string SFTP_CONNECTION_ERROR = "Could not connect to SFTP server.";
        private static string SFTP_DISCONNECTION_ERROR = "Could not disconnect from SFTP server.";

        internal static void ConnectSftpClient(this SftpClient sftpClient)
        {
            try
            {
                if (!sftpClient.IsConnected)
                    sftpClient.Connect();
            }catch(Exception ex)
            {
                throw new InvalidOperationException(SFTP_CONNECTION_ERROR, ex);
            }


            if (!sftpClient.IsConnected)
                throw new InvalidOperationException(SFTP_CONNECTION_ERROR);
        }

        internal static void DisconnectSftpClient(this SftpClient sftpClient)
        {
            try
            {
                if (sftpClient.IsConnected)
                    sftpClient.Disconnect();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(SFTP_DISCONNECTION_ERROR, ex);
            }
        }
    }
}
