# FtpSpike
# FTP Libraries Evaluation Spike

This spike evaluates using different nuget package for FTPS and SFTP file transfers.

## Development & Local Setup
The code has been developed using [.NET Core version 3.1](https://www.microsoft.com/net/download/core).

To FTPS and SFTP servers, you can execute the docker-compose file:

```
docker-compose up -d --build
```

To monitor remote server folders, logs and certificate create local folders (they are mapped in docker-compose):
d:\docker\ftps\certificate
d:\docker\ftps\logs
D:\docker\ftps\share
d:\docker\sftp\share

Before running the application create test folders:
d:\ftp\upload
d:\ftp\download

Insert some files into:
d:\ftp\upload

After running the application the files should be transferred into:
d:\ftp\download
