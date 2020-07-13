using System;

namespace ChilkatFtp.Ftp.Settings
{
    public class SessionOptions
    {
        public string HostName { get; set; }
        public int PortNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public TimeSpan Timeout { get; set; }
        
        
        /*
        HostName = configuration["FtpsOptions:Host"],
        PortNumber = int.Parse(configuration["FtpsOptions:Port"]),
        UserName = configuration["FtpsOptions:UserName"],
        Password = configuration["FtpsOptions:Password"],
        Timeout = TimeSpan.Parse(configuration["FtpsOptions:Timeout"])*/
    }
}
