using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace FluentRenciFtp.Ftp.Settings
{
    public class FtpSettingsFactory
    {
        private readonly Dictionary<FtpTypes, FtpSettings> _ftpSetup;

        public FtpSettingsFactory(IConfiguration configuration)
        {
            _ftpSetup = new Dictionary<FtpTypes, FtpSettings>()
            {
                {
                    FtpTypes.Sftp,
                    new FtpSettings()
                    {
                        SessionOptions = new SessionOptions
                        {
                            HostName = configuration["SftpOptions:Host"],
                            PortNumber = int.Parse(configuration["SftpOptions:Port"]),
                            UserName = configuration["SftpOptions:UserName"],
                            Password = configuration["SftpOptions:Password"],
                            Timeout = TimeSpan.Parse(configuration["SftpOptions:Timeout"])
                        },
                        FolderSettings = new FolderSettings()
                        {
                            LocalUploadFolder = configuration["SftpOptions:LocalUploadFolder"],
                            RemoteUploadFolder = configuration["SftpOptions:RemoteUploadFolder"],
                            LocalDownloadFolder = configuration["SftpOptions:LocalDownloadFolder"],
                            RemoteDownloadFolder = configuration["SftpOptions:RemoteDownloadFolder"]
                        }
                    }
                },
                {
                    FtpTypes.Ftps,
                    new FtpSettings()
                    {
                        SessionOptions = new SessionOptions
                        {
                            HostName = configuration["FtpsOptions:Host"],
                            PortNumber = int.Parse(configuration["FtpsOptions:Port"]),
                            UserName = configuration["FtpsOptions:UserName"],
                            Password = configuration["FtpsOptions:Password"],
                            Timeout = TimeSpan.Parse(configuration["FtpsOptions:Timeout"])
                        },
                        FolderSettings = new FolderSettings()
                        {
                            LocalUploadFolder = configuration["FtpsOptions:LocalUploadFolder"],
                            RemoteUploadFolder = configuration["FtpsOptions:RemoteUploadFolder"],
                            LocalDownloadFolder = configuration["FtpsOptions:LocalDownloadFolder"],
                            RemoteDownloadFolder = configuration["FtpsOptions:RemoteDownloadFolder"]
                        }
                    }
                }
            };
        }

        public FtpSettings Create(FtpTypes ftpType)
        {
            return _ftpSetup[ftpType];
        }
    }
}
