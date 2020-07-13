﻿using System;

namespace ChilkatFtp.Ftp.Settings
{
    public class SessionOptions
    {
        public string HostName { get; set; }
        public int PortNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
