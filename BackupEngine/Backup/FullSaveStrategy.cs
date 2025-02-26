using System;
using System.IO;
using BackupEngine.State;
using System.Linq;
using BackupEngine.Log;
using System.Text;
using System.Diagnostics;

namespace BackupEngine.Backup
{
    // The FullSaveStrategy class implements a full backup strategy. It inherits from the SaveStrategy class.
    // This strategy copies all files from the source folder to a destination folder.
    public class FullSaveStrategy : SaveStrategy
    {
        // Constructor that initializes the backup configuration
        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        // Main method that performs the full backup
        public override void Save(string uniqueDestinationPath)
        {
            // If encryption is enabled in the configuration, use the encryption strategy
            if (Configuration.Encrypt)
            {
                TransferStrategy = new CryptStrategy();
            }
            else
            {
                // Otherwise, use the copy strategy
                TransferStrategy = new CopyStrategy();
            }

            // Get the absolute path of the source folder from the configuration
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            // Check if the source folder exists
            if (!Directory.Exists(sourcePath))
            {
                // If the source folder does not exist, throw an exception
                throw new DirectoryNotFoundException($"The source folder '{sourcePath}' does not exist.");
            }

            // Retrieve all files to be backed up from the source folder
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length; // Total number of files to back up
            long totalSize = files.Sum(file => new FileInfo(file).Length); // Total size of the files to back up
            int remainingFiles = totalFiles; // Number of remaining files
            long remainingSize = totalSize; // Remaining size of the files to be backed up

            // Update the backup state before starting
            OnStateUpdated(new StateEvent(
                "Full Backup",   // Backup name
                "Active",        // Backup status (active)
                totalFiles,      // Total number of files
                totalSize,       // Total size of files
                remainingFiles,  // Remaining files to transfer
                remainingSize,   // Remaining size to transfer
                "",              // No specific file at the start
                ""               // No specific destination at the start
            ));

            // Iterate over all the files to copy them to the destination folder
            foreach (string file in files)
            {
                // Get the relative path of the file relative to the source folder
                string relativePath = file.Substring(sourcePath.Length + 1);
                // Create the destination path for this file
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                // Create the necessary directories in the destination folder
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                try
                {
                    // Measure the time it takes to transfer the file
                    DateTime start = DateTime.Now;
                    // Transfer the file using the transfer strategy
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start; // Calculate the transfer duration

                    // Update the state with the transferred file information
                    remainingFiles--;  // Decrement the remaining files count
                    remainingSize -= new FileInfo(file).Length;  // Reduce the remaining size

                    // Send a state update event with the updated information
                    OnStateUpdated(new StateEvent(
                        "Full Backup",  // Backup name
                        "Active",       // Backup status
                        totalFiles,     // Total number of files
                        totalSize,      // Total size of files
                        remainingFiles, // Remaining files
                        remainingSize,  // Remaining size
                        file,           // Source file
                        destFile        // Destination file
                    ));

                    // Create a transfer event with the duration and file information
                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    // Send the transfer event
                    OnTransfer(transferEvent);
                }
                catch (Exception e)
                {
                    // In case of an error during file copy, display an error message
                    Console.WriteLine($"Error copying file {file}: {e.Message}");
                    // Create a transfer event with a duration of -1 to indicate an error
                    OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), new FileInfo(file), new FileInfo(destFile)));
                }
            }

            // Update the state at the end of the backup
            OnStateUpdated(new StateEvent(
                "Full Backup", // Backup name
                "Completed",   // Backup status (completed)
                totalFiles,    // Total number of files
                totalSize,     // Total size of files
                0,             // No remaining files
                0,             // No remaining size
                "",            // No specific file at the end
                ""             // No specific destination at the end
            ));

            // Display a message indicating that the backup is completed
            Console.WriteLine($"Full backup completed at: {uniqueDestinationPath}");
        }
    }
}
