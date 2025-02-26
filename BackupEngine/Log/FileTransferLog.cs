using LogLib;

namespace BackupEngine.Log
{
    public class FileTransferLog : LogLib.Log  
    {
        /// <summary>
        /// CustomPath source du fichier transféré
        /// </summary> 
        public string? FileSourcePath { get; set; }

        /// <summary>
        /// CustomPath de destination du fichier transféré
        /// </summary>
        public string? FileDestinationPath { get; set; }

        /// <summary>
        /// Taille du fichier transféré (en octets)
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// Temps de transfert en millisecondes (-1 si échec)
        /// </summary>
        public int? TransferTime { get; set; }

        /// <summary>
        /// Nom du backup lié au transfert
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
