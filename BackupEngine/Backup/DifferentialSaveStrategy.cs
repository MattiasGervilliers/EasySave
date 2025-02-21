using BackupEngine.Cache;
using BackupEngine.Log;
using BackupEngine.Settings;
using BackupEngine.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupEngine.Backup
{
    public class DifferentialSaveStrategy(BackupConfiguration configuration) : SaveStrategy(configuration)
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();
        private SettingsRepository SettingsRepository = new SettingsRepository();

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
            var extensionPriorities = SettingsRepository.GetExtensionPriority();

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

            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories)
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

            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
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
                        totalFiles,
                        totalSize,
                        remainingFiles,
                        remainingSize,
                        file,
                        destFile
                    ));

                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    OnTransfer(transferEvent);
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
