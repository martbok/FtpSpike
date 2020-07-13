using System;
using System.Threading.Tasks;

namespace FluentRenciFtp.Ftp
{
    public abstract class FtpService
    {
        public abstract Task UploadFilesAsync();

        public abstract Task DownloadFilesAsync();

        protected async Task TransferSessionAsync<T>(Func<T, Task> operation, T session) where T : IDisposable
        {
            try
            {
                using (session)
                {
                    await operation(session);
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
