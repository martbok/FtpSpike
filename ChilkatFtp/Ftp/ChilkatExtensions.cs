using System;

namespace ChilkatFtp.Ftp
{
    public static class ChilkatExtensions
    {
        public static void EnsureSuccess(this bool success, string message = null)
        {
            if (!success)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
