using WinSCP;

namespace FtpSpike.Ftp.Settings
{
    public class FtpSettings
    {
        public SessionOptions SessionOptions { get; set; }
        public FolderSettings FolderSettings { get; set; }
    }
}
