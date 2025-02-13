namespace BackupEngine.Log
{
    public class TransferEvent(BackupConfiguration configuration, TimeSpan transferDuration, FileInfo file, FileInfo newFile)
    {
        public BackupConfiguration Configuration = configuration;
        public TimeSpan TransferDuration = transferDuration;
        public FileInfo File = file;
        public FileInfo NewFile = newFile;
    }
}
