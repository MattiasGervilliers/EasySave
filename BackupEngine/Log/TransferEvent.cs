namespace BackupEngine.Log
{
    public class TransferEvent(BackupConfiguration configuration, TimeSpan transferDuration, FileInfo file, FileInfo newFile)
    {
        /// <summary>
        /// The backup configuration associated with the file transfer.
        /// </summary>
        public BackupConfiguration Configuration = configuration;

        /// <summary>
        /// The duration taken to transfer the file.
        /// </summary>
        public TimeSpan TransferDuration = transferDuration;

        /// <summary>
        /// The original file being transferred.
        /// </summary>
        public FileInfo File = file;

        /// <summary>
        /// The newly created file at the destination after the transfer.
        /// </summary>
        public FileInfo NewFile = newFile;
    }
}
