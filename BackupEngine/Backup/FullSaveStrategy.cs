using System;
using System.IO;
using BackupEngine.State;
using System.Linq;
using BackupEngine.Log;
using System.Collections.Generic;
using BackupEngine.Settings;

namespace BackupEngine.Backup
{
    public class FullSaveStrategy : SaveStrategy
    {
        private SettingsRepository SettingsRepository = new SettingsRepository();

        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        public override void Save(string uniqueDestinationPath)
        {
            // Choose the transfer strategy based on encryption settings
            if (Configuration.ExtensionsToSave != null)
            {
                TransferStrategy = new CryptStrategy(Configuration.ExtensionsToSave, SettingsRepository.GetExtensionPriority());
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }

            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"The source folder '{sourcePath}' does not exist.");
            }

            // Retrieve all files to be backed up
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

            // Load extension priority list
            HashSet<string> extensionPriority = SettingsRepository.GetExtensionPriority();

            // Order files based on extension priority and depth in the directory structure
            List<string> orderedFiles = files
                .OrderBy(file => extensionPriority.Contains(Path.GetExtension(file))
                    ? extensionPriority.ToList().IndexOf(Path.GetExtension(file))
                    : int.MaxValue) // Prioritize extensions
                .ThenBy(file => file.Split(Path.DirectorySeparatorChar).Length) // Prefer shallower directories
                .ThenBy(file => file) // Ensure stable order
                .ToList();

            int totalFiles = orderedFiles.Count;
            long totalSize = orderedFiles.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            // Update the state at the beginning of the backup
            OnStateUpdated(new StateEvent(
                "Full Backup",
                "Active",
                totalFiles,
                totalSize,
                remainingFiles,
                remainingSize,
                "",
                ""
            ));

            // Process each file in the sorted order
            foreach (string file in orderedFiles)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                try
                {
                    DateTime start = DateTime.Now;
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    // Update remaining file count and size
                    remainingFiles--;
                    remainingSize -= new FileInfo(file).Length;

                    // Notify state update
                    OnStateUpdated(new StateEvent(
                        "Full Backup",
                        "Active",
                        totalFiles,
                        totalSize,
                        remainingFiles,
                        remainingSize,
                        file,
                        destFile
                    ));

                    // Log transfer event
                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    OnTransfer(transferEvent);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error copying file {file}: {e.Message}");
                    OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), new FileInfo(file), new FileInfo(destFile)));
                }
            }

            // Final state update after backup completion
            OnStateUpdated(new StateEvent(
                "Full Backup",
                "Completed",
                totalFiles,
                totalSize,
                0,
                0,
                "",
                ""
            ));

            Console.WriteLine($"Full backup completed in: {uniqueDestinationPath}");
        }
    }
}
