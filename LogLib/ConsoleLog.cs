using System;
using System.Text.Json.Serialization;

namespace LogLib
{
    // ConsoleLog hérite de Log et ajoute des informations spécifiques aux opérations de sauvegarde
    public class ConsoleLog : Log
    {
        // Nom de la sauvegarde associée au log
        public string BackupName { get; set; }

        // Chemin source du fichier sauvegardé
        public string SourcePath { get; set; }

        // Chemin destination où le fichier est sauvegardé
        public string DestinationPath { get; set; }

        // Taille du fichier en octets
        public long FileSize { get; set; }

        // Temps de transfert en millisecondes
        public long TransferTimeMs { get; set; }

        // Constructeur qui initialise les propriétés avec des valeurs fournies
        public ConsoleLog(LogLevel level, string message, string backupName, string sourcePath, string destinationPath, long fileSize, long transferTimeMs)
            : base(level, message) // Appelle le constructeur de la classe parente Log
        {
            BackupName = backupName;
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            FileSize = fileSize;
            TransferTimeMs = transferTimeMs;
        }
    }
}
