using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Progress
{
    public class StateEvent
    {
        public string JobName { get; set; }  // Appellation du travail de sauvegarde
        public DateTime LastActionTimestamp { get; set; }  // Horodatage de la dernière action
        public string JobState { get; set; }  // État du travail de sauvegarde (ex : Actif, Non Actif...)

        // Informations sur la sauvegarde
        public int TotalEligibleFiles { get; set; }  // Nombre total de fichiers éligibles
        public long TotalSizeToTransfer { get; set; }  // Taille totale des fichiers à transférer

        // Progression
        public int RemainingFiles { get; set; }  // Nombre de fichiers restants
        public long RemainingSize { get; set; }  // Taille des fichiers restants
        public string CurrentSourceFile { get; set; }  // Adresse complète du fichier source en cours de sauvegarde
        public string CurrentDestinationFile { get; set; }  // Adresse complète du fichier de destination

        public StateEvent(string jobName, string jobState, int totalEligibleFiles, long totalSizeToTransfer, int remainingFiles, long remainingSize, string currentSourceFile, string currentDestinationFile)
        {
            JobName = jobName;
            JobState = jobState;
            TotalEligibleFiles = totalEligibleFiles;
            TotalSizeToTransfer = totalSizeToTransfer;
            LastActionTimestamp = DateTime.Now;
            RemainingFiles = remainingFiles;
            RemainingSize = remainingSize;
            CurrentSourceFile = currentSourceFile;
            CurrentDestinationFile = currentDestinationFile;
        }
    }

}
