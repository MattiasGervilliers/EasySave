using LogLib;
using BackupEngine.Settings;

namespace BackupEngine.Log
{
    /// <summary>
    /// La classe FileTransferLogManager est responsable de la gestion des logs liés au transfert de fichiers.
    /// Elle utilise un LogWriter pour enregistrer les événements de transfert dans des fichiers de log.
    /// </summary>
    internal class FileTransferLogManager
    {
        /// <summary>
        /// Instance de LogWriter qui est utilisée pour écrire les logs dans le fichier de log spécifié.
        /// </summary>
        private readonly LogWriter _logWriter;

        /// <summary>
        /// Constructeur de la classe FileTransferLogManager.
        /// Initialisation de LogWriter avec le chemin du répertoire des logs et le type de log spécifié.
        /// </summary>
        /// <param name="logDirectoryPath">Chemin du répertoire où les logs seront stockés.</param>
        /// <param name="logType">Type de log (par exemple, fichier texte ou autre format).</param>
        public FileTransferLogManager(string logDirectoryPath, LogType logType)
        {
            /// <summary>
            /// Création d'une instance de LogWriter en lui passant le répertoire de logs et le type de log.
            /// </summary>
            _logWriter = new LogWriter(logDirectoryPath, logType);
        }

        /// <summary>
        /// Méthode qui est appelée lors du transfert d'un fichier pour enregistrer les détails de ce transfert dans un log.
        /// </summary>
        /// <param name="sender">L'objet émetteur de l'événement (habituellement l'objet déclencheur du transfert).</param>
        /// <param name="transferEvent">L'événement qui contient les informations relatives au transfert (comme les chemins, taille, durée, etc.).</param>
        public void OnTransfer(object sender, TransferEvent transferEvent)
        {
            /// <summary>
            /// Récupère le chemin complet du fichier source transféré.
            /// </summary>
            string sourcePath = transferEvent.File.FullName;

            /// <summary>
            /// Récupère le chemin complet du fichier de destination du transfert.
            /// </summary>
            string destinationPath = transferEvent.NewFile.FullName;

            /// <summary>
            /// Récupère la taille du fichier transféré en octets.
            /// </summary>
            long size = transferEvent.File.Length;

            /// <summary>
            /// Récupère la durée du transfert en millisecondes.
            /// </summary>
            int duration = transferEvent.TransferDuration.Milliseconds;

            /// <summary>
            /// Récupère le nom de la configuration de la sauvegarde associée au transfert.
            /// </summary>
            string backupName = transferEvent.Configuration.Name;

            /// <summary>
            /// Crée un objet FileTransferLog avec les informations du transfert et écrit ce log dans le fichier à l'aide du LogWriter.
            /// </summary>
            _logWriter.WriteLog(new FileTransferLog(
                    sourcePath,
                    destinationPath,
                    size,
                    duration,
                    backupName
                )
            );
        }
    }
}

