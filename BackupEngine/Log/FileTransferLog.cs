using LogLib;

namespace BackupEngine.Log
{
    internal class FileTransferLog : LogLib.Log
    {
        /// <summary>
        /// Chemin source du fichier transféré
        /// </summary>
        public string FileSourcePath { get; set; }

        /// <summary>
        /// Chemin de destination du fichier transféré
        /// </summary>
        public string FileDestinationPath { get; set; }

        /// <summary>
        /// Taille du fichier transféré (en octets)
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Temps de transfert en millisecondes (-1 si échec)
        /// </summary>
        public int TransferTime { get; set; }

        /// <summary>
        /// Nom du backup lié au transfert
        /// </summary>
        public string BackupName { get; set; }

        public FileTransferLog(string fileSourcePath, string fileDestinationPath, long fileSize, int transferTime, string backupName)
            : base(LogLevel.INFO, "")
        {
            Message = $"Transfert du fichier {fileSourcePath} vers {fileDestinationPath} ({fileSize} octets) en {transferTime} ms";
        }
    }
}
