using BackupEngine.Cache;
using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    // The DifferentialSaveStrategy class inherits from the SaveStrategy class and implements a differential backup strategy
    // It performs a backup of only the files that have changed since the last backup
    public class DifferentialSaveStrategy : SaveStrategy
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();

        // Main method of the differential backup strategy
        public override void Save(string uniqueDestinationPath)
        {
            // If a previous backup exists, perform a differential backup
            if (PreviousSaveExists())
            {
                string previousSavePath = PreviousSavePath();

                // If the previous backup does not exist (missing folder), perform a full backup
                if (!Directory.Exists(previousSavePath))
                {
                    PerformFullSave(uniqueDestinationPath);
                    UpdateCache(uniqueDestinationPath);
                }
                else
                {
                    // If the previous backup exists, perform a differential backup
                    DifferentialSave(uniqueDestinationPath, previousSavePath);
                }
            }
            else
            {
                // If no previous backup exists, perform a full backup
                PerformFullSave(uniqueDestinationPath);
                UpdateCache(uniqueDestinationPath);
            }
        }

        // Perform a full backup by calling the full backup strategy
        private void PerformFullSave(string uniqueDestinationPath)
        {
            FullSaveStrategy fullSaveStrategy = new FullSaveStrategy(Configuration);
            // Subscribe to the transfer and state update events
            fullSaveStrategy.Transfer += (sender, e) => OnTransfer(e);
            fullSaveStrategy.StateUpdated += (sender, e) => OnStateUpdated(e);
            // Execute the full backup
            fullSaveStrategy.Save(uniqueDestinationPath);
        }

        // Perform a differential backup by comparing files with the previous backup
        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath)
        {
            // Choose the transfer strategy based on the configuration (encryption or copy)
            if (Configuration.Encrypt)
            {
                TransferStrategy = new CryptStrategy();
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }

            // Get the absolute path of the source folder
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            // Check if the source folder exists
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"The source folder '{sourcePath}' does not exist.");
            }

            // Create the destination folder if necessary
            Directory.CreateDirectory(uniqueDestinationPath);

            // Retrieve the list of all files in the source folder
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            // Update the state with initial backup information
            OnStateUpdated(new StateEvent(
                "Differential Backup",
                "Active",
                totalFiles,
                totalSize,
                remainingFiles,
                remainingSize,
                "",
                ""
            ));

            // Process each file in the source folder
            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string prevFile = Path.Combine(previousSavePath, relativePath);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);

                // Create the necessary subdirectories in the destination folder
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                // Check if the file has changed since the last backup
                bool fileDoesNotExistInPrevious = !File.Exists(prevFile);
                bool fileHasChanged = fileDoesNotExistInPrevious || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(prevFile);

                if (fileHasChanged)
                {
                    DateTime start = DateTime.Now;
                    // Transfer the file if necessary
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    // Update the state with the information of the file being transferred
                    remainingFiles--;
                    remainingSize -= new FileInfo(file).Length;

                    // Send the state event for this file
                    OnStateUpdated(new StateEvent(
                        "Differential Backup",
                        "Active",
                        totalFiles,
                        totalSize,
                        remainingFiles,
                        remainingSize,
                        file,
                        destFile
                    ));

                    // Create and send the transfer event
                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    OnTransfer(transferEvent);
                }
            }

            // Update the state at the end of the backup
            OnStateUpdated(new StateEvent(
                "Differential Backup",
                "Completed",
                totalFiles,
                totalSize,
                0,
                0,
                "",
                ""
            ));

            // Display a confirmation message in the console
            Console.WriteLine($"Differential backup completed in: {uniqueDestinationPath}");
            // Update the cache (commented here)
            //UpdateCache(uniqueDestinationPath);
        }

        // Check if a previous backup exists using the cache
        private bool PreviousSaveExists()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached != null;
        }

        // Retrieve the path of the last backup from the cache
        private string PreviousSavePath()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            // Get the last backup directory
            return cached!.Backups.OrderByDescending(b => b.Date).First().DirectoryName;
        }

        // Update the cache with the information of the new backup
        private void UpdateCache(string uniqueDestinationPath)
        {
            _cacheRepository.AddBackup(Configuration, DateTime.Now, uniqueDestinationPath);
        }
    }
}
