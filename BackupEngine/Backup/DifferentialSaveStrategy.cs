using BackupEngine.Cache;
using BackupEngine.Log;
using BackupEngine.Settings;
using BackupEngine.State;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    public class DifferentialSaveStrategy(BackupConfiguration configuration) : SaveStrategy(configuration)
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();
        private SettingsRepository _settingsRepository = new SettingsRepository();
        private readonly int _koLimit = 1024; 
        private readonly SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);

        public override void Save(string uniqueDestinationPath)
        {
            if (PreviousSaveExists())
            {
                string previousSavePath = PreviousSavePath();

                if (!Directory.Exists(previousSavePath))
                {
                    PerformFullSave(uniqueDestinationPath);
                    UpdateCache(uniqueDestinationPath);
                }
                else
                {
                    DifferentialSave(uniqueDestinationPath, previousSavePath);
                }
            }
            else
            {
                PerformFullSave(uniqueDestinationPath);
                UpdateCache(uniqueDestinationPath);
            }
        }

        private void PerformFullSave(string uniqueDestinationPath)
        {
            FullSaveStrategy fullSaveStrategy = new FullSaveStrategy(Configuration);
            fullSaveStrategy.Transfer += (sender, e) => OnTransfer(e);
            fullSaveStrategy.StateUpdated += (sender, e) => OnStateUpdated(e);
            fullSaveStrategy.Save(uniqueDestinationPath);
        }

        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath)
        {
            var extensionPriorities = _settingsRepository.GetExtensionPriority();

            if (Configuration.ExtensionsToSave != null)
            {
                TransferStrategy = new CryptStrategy(Configuration.ExtensionsToSave, extensionPriorities);
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }

            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            Directory.CreateDirectory(uniqueDestinationPath);

            var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories)
                                 .OrderBy(f => extensionPriorities.Contains(Path.GetExtension(f)) ? 0 : 1)
                                 .ToArray();

            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

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

            var smallFilesQueue = new ConcurrentQueue<string>();
            var largeFilesQueue = new Queue<string>();

            foreach (var file in files)
            {
                if (new FileInfo(file).Length <= _koLimit * 1024)
                {
                    smallFilesQueue.Enqueue(file);
                }
                else
                {
                    largeFilesQueue.Enqueue(file);
                }
            }

            Parallel.ForEach(smallFilesQueue, file => TransferFile(file, previousSavePath, uniqueDestinationPath, ref remainingFiles, ref remainingSize));

            foreach (var file in largeFilesQueue)
            {
                _largeFileSemaphore.Wait();
                try
                {
                    Console.WriteLine(file);

                    TransferFile(file, previousSavePath, uniqueDestinationPath, ref remainingFiles, ref remainingSize);
                }
                finally
                {
                    _largeFileSemaphore.Release();
                }
            }

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

            Console.WriteLine($"Sauvegarde différentielle effectuée dans : {uniqueDestinationPath}");
        }

        private void TransferFile(string file, string previousSavePath, string uniqueDestinationPath, ref int remainingFiles, ref long remainingSize)
        {
            string relativePath = file.Substring(Configuration.SourcePath.GetAbsolutePath().Length + 1);
            string prevFile = Path.Combine(previousSavePath, relativePath);
            string destFile = Path.Combine(uniqueDestinationPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            bool fileDoesNotExistInPrevious = !File.Exists(prevFile);
            bool fileHasChanged = fileDoesNotExistInPrevious || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(prevFile);

            if (fileHasChanged)
            {
                DateTime start = DateTime.Now;
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= new FileInfo(file).Length;

                OnStateUpdated(new StateEvent(
                    "Differential Backup",
                    "Active",
                    remainingFiles,
                    remainingSize,
                    remainingFiles,
                    remainingSize,
                    file,
                    destFile
                ));

                TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                OnTransfer(transferEvent);
            }
        }

        private bool PreviousSaveExists()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached != null;
        }

        private string PreviousSavePath()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached!.Backups.OrderByDescending(b => b.Date).First().DirectoryName;
        }

        private void UpdateCache(string uniqueDestinationPath)
        {
            _cacheRepository.AddBackup(Configuration, DateTime.Now, uniqueDestinationPath);
        }
    }
}
