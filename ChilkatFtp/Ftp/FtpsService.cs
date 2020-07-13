using System;
using ChilkatFtp.Ftp.Settings;

namespace ChilkatFtp.Ftp
{
    public class FtpsService : FtpService
    {
        private readonly FtpSettings _ftpSettings;

        public FtpsService(FtpSettings ftpSettings)
        {
            _ftpSettings = ftpSettings;
        }

        public override void UploadFiles()
        {
            Console.WriteLine("Uploading files:");
            TransferFiles((session) =>
            {
                session.ChangeRemoteDir(_ftpSettings.FolderSettings.RemoteUploadFolder).EnsureSuccess(session.LastErrorText);
                session.SyncRemoteTree2(_ftpSettings.FolderSettings.LocalUploadFolder, (int)FileSyncMode.All, false, false).EnsureSuccess(session.LastErrorText);
            });
        }

        public override void DownloadFiles()
        {
            Console.WriteLine("Downloading files:");
            TransferFiles((session) =>
            {
                session.ChangeRemoteDir(_ftpSettings.FolderSettings.RemoteDownloadFolder).EnsureSuccess(session.LastErrorText);
                session.SyncLocalDir(_ftpSettings.FolderSettings.LocalDownloadFolder, (int) FileSyncMode.All).EnsureSuccess(session.LastErrorText);
            });
        }

        private void TransferFiles(Action<Chilkat.Ftp2> operation)
        {
            TransferSession<Chilkat.Ftp2>((session) =>
            {
                session.ConnectTimeout = _ftpSettings.SessionOptions.Timeout.Milliseconds;

                session.Hostname = _ftpSettings.SessionOptions.HostName;
                session.Port = _ftpSettings.SessionOptions.PortNumber;
                session.Username = _ftpSettings.SessionOptions.UserName;
                session.Password = _ftpSettings.SessionOptions.Password;
                session.Passive = true;
                session.AuthTls = true;
                session.Ssl = false;

                session.Connect().EnsureSuccess(session.LastErrorText);
                operation(session);

                Console.WriteLine($"Transferred:{Environment.NewLine}{session.SyncedFiles}");
            });
        }
    }
}
