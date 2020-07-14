using System;
using System.Threading.Tasks;
using FluentRenciFtp.Ftp;
using FluentRenciFtp.Ftp.Settings;
using Microsoft.Extensions.Configuration;

namespace FluentRenciFtp
{
    public class Program
    {
        private const string AppSettingsFileName = "appsettings.json";

        public static async Task Main(string[] args)
        {
            var appSettings = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsFileName, true, true)
                .AddEnvironmentVariables(prefix: "CPFileBridge_")
                .Build();
            var ftpSettingsFactory = new FtpSettingsFactory(appSettings);

            Console.WriteLine("== SFTP testing ==");
            var sftpService = new SftpService(ftpSettingsFactory.Create(FtpTypes.Sftp));
            await sftpService.UploadFilesAsync();
            await sftpService.DownloadFilesAsync();

            Console.WriteLine("== FTPS testing ==");
            var ftpsService = new FtpsService(ftpSettingsFactory.Create(FtpTypes.Ftps));
            await ftpsService.UploadFilesAsync();
            await ftpsService.DownloadFilesAsync();

            Console.WriteLine("Ftp tests completed.");
        }
    }
}
