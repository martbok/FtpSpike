using System;

namespace ChilkatFtp.Ftp
{
    public abstract class FtpService
    {
        public abstract void UploadFiles();

        public abstract void DownloadFiles();

        protected void TransferSession<T>(Action<T> operation) where T : IDisposable, new()
        {
            try
            {
                using (var session = new T())
                {
                    operation(session);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
