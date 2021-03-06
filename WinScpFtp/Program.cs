﻿using System;
using Microsoft.Extensions.Configuration;
using WinScpFtp.Ftp;
using WinScpFtp.Ftp.Settings;

namespace WinScpFtp
{
    public class Program
    {
        private const string AppSettingsFileName = "appsettings.json";

        public static void Main(string[] args)
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile(AppSettingsFileName).Build();
            var ftpSettingsFactory = new FtpSettingsFactory(appSettings);

            Console.WriteLine("== SFTP testing ==");
            var sftpService = new FtpService(ftpSettingsFactory.Create(FtpTypes.Sftp));
            sftpService.UploadFiles();
            sftpService.DownloadFiles();

            Console.WriteLine("== FTPS testing ==");
            var ftpsService = new FtpService(ftpSettingsFactory.Create(FtpTypes.Ftps));
            ftpsService.UploadFiles();
            ftpsService.DownloadFiles();

            Console.WriteLine("Ftp tests completed.");
        }
    }
}
