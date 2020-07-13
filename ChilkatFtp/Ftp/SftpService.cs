using System;
using ChilkatFtp.Ftp.Settings;

namespace ChilkatFtp.Ftp
{
    public class SftpService : FtpService
    {
        private readonly FtpSettings _ftpSettings;

        public SftpService(FtpSettings ftpSettings)
        {
            _ftpSettings = ftpSettings;
        }

        public override void UploadFiles()
        {
            Console.WriteLine("Uploading files:");
            TransferFiles((session) => 
                session.SyncTreeUpload(_ftpSettings.FolderSettings.LocalUploadFolder, _ftpSettings.FolderSettings.RemoteUploadFolder, (int)FileSyncMode.All, false).EnsureSuccess(session.LastErrorText)
            );
        }

        public override void DownloadFiles()
        {
            Console.WriteLine("Downloading files:");
            TransferFiles((session) =>
                session.SyncTreeDownload(_ftpSettings.FolderSettings.RemoteDownloadFolder, _ftpSettings.FolderSettings.LocalDownloadFolder, (int)FileSyncMode.All, false).EnsureSuccess(session.LastErrorText)
            );
        }

        private void TransferFiles(Action<Chilkat.SFtp> operation)
        {
            TransferSession<Chilkat.SFtp>((session) =>
            {
                session.ConnectTimeoutMs = _ftpSettings.SessionOptions.Timeout.Milliseconds;

                session.Connect(_ftpSettings.SessionOptions.HostName, _ftpSettings.SessionOptions.PortNumber).EnsureSuccess(session.LastErrorText);
                session.AuthenticatePw(_ftpSettings.SessionOptions.UserName, _ftpSettings.SessionOptions.Password).EnsureSuccess(session.LastErrorText);

                session.InitializeSftp().EnsureSuccess(session.LastErrorText);
                operation(session);

                Console.WriteLine($"Transferred:{Environment.NewLine}{session.SyncedFiles}");
            });
        }
    }
}
