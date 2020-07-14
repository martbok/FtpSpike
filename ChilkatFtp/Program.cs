using System;
using ChilkatFtp.Ftp;
using ChilkatFtp.Ftp.Settings;
using Microsoft.Extensions.Configuration;

namespace ChilkatFtp
{
    public class Program
    {
        private const string AppSettingsFileName = "appsettings.json";

        public static void Main(string[] args)
        {
            Unlock();

            var appSettings = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsFileName, true, true)
                .AddEnvironmentVariables(prefix: "CPFileBridge_")
                .Build();
            var ftpSettingsFactory = new FtpSettingsFactory(appSettings);

            Console.WriteLine("== SFTP testing ==");
            var sftpService = new SftpService(ftpSettingsFactory.Create(FtpTypes.Sftp));
            sftpService.UploadFiles();
            sftpService.DownloadFiles();

            Console.WriteLine("== FTPS testing ==");
            var ftpsService = new FtpsService(ftpSettingsFactory.Create(FtpTypes.Ftps));
            ftpsService.UploadFiles();
            ftpsService.DownloadFiles();

            Console.WriteLine("Ftp tests completed.");
        }

        private static void Unlock()
        {
            // The Chilkat API can be unlocked for a fully-functional 30-day trial by passing any
            // string to the UnlockBundle method.  A program can unlock once at the start. Once unlocked,
            // all subsequently instantiated objects are created in the unlocked state. 
            // 
            // After licensing Chilkat, replace the "Anything for 30-day trial" with the purchased unlock code.
            // To verify the purchased unlock code was recognized, examine the contents of the LastErrorText
            // property after unlocking.  For example:
            Chilkat.Global glob = new Chilkat.Global();
            bool success = glob.UnlockBundle("Anything for 30-day trial");
            if (success != true)
            {
                Console.WriteLine(glob.LastErrorText);
                return;
            }

            int status = glob.UnlockStatus;
            if (status == 2)
            {
                Console.WriteLine("Unlocked using purchased unlock code.");
            }
            else
            {
                Console.WriteLine("Unlocked in trial mode.");
            }

            // The LastErrorText can be examined in the success case to see if it was unlocked in
            // trial more, or with a purchased unlock code.
            Console.WriteLine(glob.LastErrorText);
        }
    }
}
