using System;
using FtpSpike.Ftp.Settings;
using WinSCP;

namespace FtpSpike.Ftp
{
    public class FtpService
    {
        private readonly FtpSettings _ftpSettings;

        private TransferOptions TransferOptions => new TransferOptions {TransferMode = TransferMode.Binary};

    public FtpService(FtpSettings ftpSettings)
        {
            _ftpSettings = ftpSettings;
        }

        public void UploadFiles()
        {
            Console.WriteLine("Uploading files:");
            TransferSession((session) => session.PutFiles(_ftpSettings.FolderSettings.LocalUploadFolder, _ftpSettings.FolderSettings.RemoteUploadFolder, false, TransferOptions));
        }

        public void DownloadFiles()
        {
            Console.WriteLine("Downloading files:");
            TransferSession((session) => session.GetFiles(_ftpSettings.FolderSettings.RemoteDownloadFolder, _ftpSettings.FolderSettings.LocalDownloadFolder, false, TransferOptions));
        }

        private void TransferSession(Func<Session, TransferOperationResult> operation)
        {
            Session session = null;
            try
            {
                using (session = new Session())
                {
                    // Connect
                    session.Open(_ftpSettings.SessionOptions);

                    TransferOperationResult transferResult = operation(session);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Transfer of {0} succeeded", transfer.FileName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                session?.Dispose();
            }
        }
    }
}
