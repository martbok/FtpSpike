using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentRenciFtp.Ftp.Settings;
using Renci.SshNet;
using Renci.SshNet.Async;
using Renci.SshNet.Sftp;

namespace FluentRenciFtp.Ftp
{
    public class SftpService : FtpService
    {
        private readonly FtpSettings _ftpSettings;

        public SftpService(FtpSettings ftpSettings) 
        {
            _ftpSettings = ftpSettings;
        }

        public override async Task UploadFilesAsync()
        {
            Console.WriteLine("Uploading files:");
            await TransferFilesAsync(async (session) =>
            {
                await CleanDirectoryAsync(session);

                var transfers = await session.SynchronizeDirectoriesAsync(_ftpSettings.FolderSettings.LocalUploadFolder, _ftpSettings.FolderSettings.RemoteUploadFolder, "*");

                foreach (var transfer in transfers)
                {
                    Console.WriteLine("Upload of {0} succeeded", transfer.Name);
                }
            });
        }

        public override async Task DownloadFilesAsync()
        {
            Console.WriteLine("Downloading files:");
            await TransferFilesAsync(async (session) =>
                await DownloadDirectoryAsync(session, _ftpSettings.FolderSettings.RemoteDownloadFolder, _ftpSettings.FolderSettings.LocalDownloadFolder, false));
        }

        private async Task TransferFilesAsync(Func<SftpClient, Task> operation)
        {
            await TransferSessionAsync<SftpClient>(async (session) =>
            {
                session.Connect();
                session.OperationTimeout = _ftpSettings.SessionOptions.Timeout;

                await operation(session);

                session.Disconnect();
            }, new SftpClient(_ftpSettings.SessionOptions.HostName, _ftpSettings.SessionOptions.PortNumber, _ftpSettings.SessionOptions.UserName, _ftpSettings.SessionOptions.Password));
        }

        private async Task CleanDirectoryAsync(SftpClient session)
        {
            IEnumerable<SftpFile> listing = await session.ListDirectoryAsync(_ftpSettings.FolderSettings.RemoteUploadFolder);

            foreach (var file in listing)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    session.DeleteFile(file.FullName);
                }
            }
        }

        private static async Task DownloadDirectoryAsync(SftpClient client, string source, string destination, bool recursive = false)
        {
            var files = await client.ListDirectoryAsync(source);

            foreach (SftpFile file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    await DownloadFileAsync(client, file, destination);
                }
                else if (file.IsSymbolicLink)
                {
                    Console.WriteLine("Symbolic link ignored: {0}", file.FullName);
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                    if (recursive)
                    {
                        await DownloadDirectoryAsync(client, file.FullName, dir.FullName);
                    }
                }
            }
        }
        private static async Task DownloadFileAsync(SftpClient client, SftpFile file, string directory)
        {
            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                await client.DownloadAsync(file.FullName, fileStream);
            }

            Console.WriteLine("Download of {0} succeeded", file.Name);
        }
    }
}
