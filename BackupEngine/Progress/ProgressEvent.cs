namespace BackupEngine.Progress
{
    public class ProgressEvent
    {
        /// <summary>
        /// Total size of files to be transferred
        /// </summary>
        public long TotalSize { get; set; } 

        /// <summary>
        /// Remaining size of files to be transferred
        /// </summary>
        public long RemainingSize { get; set; }  // Taille des fichiers restants
        
        public ProgressEvent(long totalSize, long remainingSize)
        {
            TotalSize = totalSize;
            RemainingSize = remainingSize;
        }
    }

}
