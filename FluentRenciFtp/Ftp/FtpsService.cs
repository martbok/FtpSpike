using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentFTP;
using FluentRenciFtp.Ftp.Settings;

namespace FluentRenciFtp.Ftp
{
    public class FtpsService : FtpService
    {
        private readonly FtpSettings _ftpSettings;

        public FtpsService(FtpSettings ftpSettings)
        {
            _ftpSettings = ftpSettings;
        }

        public override async Task UploadFilesAsync()
        {
            Console.WriteLine("Uploading files:");
            await TransferFilesAsync(async (session) => 
                await session.UploadDirectoryAsync(_ftpSettings.FolderSettings.LocalUploadFolder, _ftpSettings.FolderSettings.RemoteUploadFolder, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite));
        }

        public override async Task DownloadFilesAsync()
        {
            Console.WriteLine("Downloading files:");
            await TransferFilesAsync(async (session) =>
                await session.DownloadDirectoryAsync(_ftpSettings.FolderSettings.LocalDownloadFolder, _ftpSettings.FolderSettings.RemoteDownloadFolder, FtpFolderSyncMode.Update, FtpLocalExists.Overwrite));
        }

        private async Task TransferFilesAsync(Func<FtpClient, Task<List<FtpResult>>> operation)
        {
            await TransferSessionAsync(async (session) =>
            {
                session.ConnectTimeout = _ftpSettings.SessionOptions.Timeout.Milliseconds;
                session.EncryptionMode = FtpEncryptionMode.Explicit;
                session.DataConnectionType = FtpDataConnectionType.PASV;
                session.ValidateCertificate += (control, e) =>
                {
                    e.Accept = true;
                };

                await session.ConnectAsync();

                var transfers = await operation(session);

                foreach (var transfer in transfers.Where(x => x.IsSuccess))
                {
                    Console.WriteLine("Transfer of {0} succeeded", transfer.Name);
                }

                await session.DisconnectAsync();
            }, new FtpClient(_ftpSettings.SessionOptions.HostName, _ftpSettings.SessionOptions.PortNumber, _ftpSettings.SessionOptions.UserName, _ftpSettings.SessionOptions.Password));
        }
    }
}
