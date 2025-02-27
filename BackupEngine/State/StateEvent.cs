using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.State
{
    public class StateEvent
    {
        public string JobName { get; set; }  // Name of the backup job
        public DateTime LastActionTimestamp { get; set; }  // Timestamp of the last action
        public string JobState { get; set; }  // State of the backup job (e.g., Active, Inactive...)

        // Backup information
        public int TotalEligibleFiles { get; set; }  // Total number of eligible files
        public long TotalSizeToTransfer { get; set; }  // Total size of files to transfer

        // Progress
        public int RemainingFiles { get; set; }  // Number of remaining files
        public long RemainingSize { get; set; }  // Size of remaining files
        public string CurrentSourceFile { get; set; }  // Full address of the current source file being backed up
        public string CurrentDestinationFile { get; set; }  // Full address of the destination file

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
