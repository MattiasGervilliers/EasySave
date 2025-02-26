using LogLib;

namespace BackupEngine.Log
{
    public class FileTransferLog : LogLib.Log
    {
        /// <summary>
        /// CustomPath of the source file transferred
        /// </summary> 
        public string? FileSourcePath { get; set; }

        /// <summary>
        /// CustomPath of the destination of the transferred file
        /// </summary>
        public string? FileDestinationPath { get; set; }

        /// <summary>
        /// Size of the transferred file (in bytes)
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// Transfer time in milliseconds (-1 if failed)
        /// </summary>
        public int? TransferTime { get; set; }

        /// <summary>
        /// Name of the backup related to the transfer
        /// </summary>
        public string? BackupName { get; set; }


        public FileTransferLog() : base(LogLevel.INFO, "")
        {
        }

        public FileTransferLog(string fileSourcePath, string fileDestinationPath, long fileSize, int transferTime, string backupName)
            : base(LogLevel.INFO, "")
        {
            FileSourcePath = fileSourcePath;
            FileDestinationPath = fileDestinationPath;
            FileSize = fileSize;
            TransferTime = transferTime;
            BackupName = backupName;
        }
    }
}
