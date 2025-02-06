using BackupEngine.Backup;
using System;

namespace BackupEngine.Log
{
    public class TransferEvent(BackupConfiguration configuration, TimeSpan transferDuration, FileInfo file)
    {
        public BackupConfiguration Configuration = configuration;
        public TimeSpan TransferDuration = transferDuration;
        public FileInfo File = file;
    }
}
