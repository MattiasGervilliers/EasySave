using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using BackupEngine.State;
using BackupEngine.Log;
using BackupEngine.Settings;

namespace BackupEngine.Backup
{
    public class FullSaveStrategy : SaveStrategy
    {
        private SettingsRepository SettingsRepository = new SettingsRepository();
        private readonly int _koLimit = 1024; 
        private readonly SemaphoreSlim largeFileSemaphore = new SemaphoreSlim(1, 1);

        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        public override void Save(string uniqueDestinationPath)
        {
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

            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            HashSet<string> extensionPriority = SettingsRepository.GetExtensionPriority();

            List<string> orderedFiles = files
                .OrderBy(file => extensionPriority.Contains(Path.GetExtension(file))
                    ? extensionPriority.ToList().IndexOf(Path.GetExtension(file))
                    : int.MaxValue)
                .ThenBy(file => file.Split(Path.DirectorySeparatorChar).Length)
                .ThenBy(file => file)
                .ToList();

            int totalFiles = orderedFiles.Count;
            long totalSize = orderedFiles.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

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

            List<Task> tasks = new List<Task>();

            foreach (string file in orderedFiles)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                FileInfo fileInfo = new FileInfo(file);
                bool isLargeFile = fileInfo.Length > _koLimit * 1024;

                Task transferTask = Task.Run(async () =>
                {
                    if (isLargeFile)
                    {
                        await largeFileSemaphore.WaitAsync();
                    }

                    try
                    {
                        DateTime start = DateTime.Now;
                        TransferStrategy.TransferFile(file, destFile);
                        DateTime end = DateTime.Now;
                        TimeSpan duration = end - start;

                        remainingFiles--;
                        remainingSize -= fileInfo.Length;

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

                        TransferEvent transferEvent = new TransferEvent(Configuration, duration, fileInfo, new FileInfo(destFile));
                        OnTransfer(transferEvent);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error copying file {file}: {e.Message}");
                        OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), fileInfo, new FileInfo(destFile)));
                    }
                    finally
                    {
                        if (isLargeFile)
                        {
                            largeFileSemaphore.Release();
                        }
                    }
                });

                tasks.Add(transferTask);
            }

            Task.WhenAll(tasks).Wait();

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
