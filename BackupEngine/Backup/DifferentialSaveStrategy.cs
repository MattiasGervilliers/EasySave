using BackupEngine.State;
using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Cache;
using System.IO;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Implements the differential backup strategy.
    /// </summary>
    public class DifferentialSaveStrategy : SaveStrategy
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();

        /// <summary>
        /// Initializes a new instance of the DifferentialSaveStrategy class.
        /// </summary>
        public DifferentialSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Executes the differential backup process.
        /// </summary>
        public override void Save(string uniqueDestinationPath, EventWaitHandle waitHandle)
        {
            if (PreviousSaveExists())
            {
                string previousSavePath = PreviousSavePath();

                if (!Directory.Exists(previousSavePath))
                {
                    PerformFullSave(uniqueDestinationPath, waitHandle);
                    UpdateCache(uniqueDestinationPath);
                }
                else
                {
                    DifferentialSave(uniqueDestinationPath, previousSavePath, waitHandle);
                }
            }
            else
            {
                PerformFullSave(uniqueDestinationPath, waitHandle);
                UpdateCache(uniqueDestinationPath);
            }
        }

        private void PerformFullSave(string uniqueDestinationPath, EventWaitHandle waitHandle)
        {
            FullSaveStrategy fullSaveStrategy = new FullSaveStrategy(_configuration);
            fullSaveStrategy.Transfer += (sender, e) => OnTransfer(e);
            fullSaveStrategy.StateUpdated += (sender, e) => OnStateUpdated(e);
            fullSaveStrategy.Progress += (sender, e) => OnProgress(e);
            fullSaveStrategy.Save(uniqueDestinationPath, waitHandle);
        }

        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath, EventWaitHandle waitHandle)
        {
            if (_configuration.ExtensionsToSave != null)
            {
                TransferStrategy = new CryptStrategy(_configuration.ExtensionsToSave, _settingsRepository.GetExtensionPriority());
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }

            string sourcePath = _configuration.SourcePath.GetAbsolutePath();
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"The source folder '{sourcePath}' does not exist.");
            }

            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            OnStateUpdated(new StateEvent("Differential Backup", "Active", totalFiles, totalSize, remainingFiles, remainingSize, "", ""));
            OnProgress(new ProgressEvent(totalSize, remainingSize));

            List<Task> tasks = new List<Task>();

            WaitForBusinessSoftwareToClose();

            OnProgress(new ProgressEvent(
                totalSize,
                remainingSize
            ));

            foreach (string file in files)
            {
                waitHandle.WaitOne();
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                string previousFile = Path.Combine(previousSavePath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                if (!ShouldBackupFile(file, previousFile)) 
                {
                    continue;
                }

                if (RequiresEncryption(file))
                {
                    _cryptoQueue.Enqueue((file, destFile));
                }
                else
                {
                    tasks.Add(Task.Run(() =>
                    {
                        WaitForBusinessSoftwareToClose();
                        TransferFile(file, destFile, ref totalSize, ref remainingFiles, ref remainingSize, ref waitHandle);
                    }));
                }
            }

            _cryptoTask = Task.Run(() =>
            {
                WaitForBusinessSoftwareToClose();
                ProcessCryptoQueue();
            });

            Task.WhenAll(tasks).Wait();
            _cryptoTask.Wait();

            OnStateUpdated(new StateEvent("Differential Backup", "Completed", totalFiles, totalSize, 0, 0, "", ""));
            OnProgress(new ProgressEvent(totalSize, 0));
            Console.WriteLine($"Differential backup completed in: {uniqueDestinationPath}");
        }

        /// <summary>
        /// Transfers a file and updates the backup state.
        /// </summary>
        private void TransferFile(string file, string destFile, ref long totalSize, ref int remainingFiles, ref long remainingSize, ref EventWaitHandle waitHandle)
        {
            FileInfo fileInfo = new FileInfo(file);
            bool isLargeFile = fileInfo.Length > _koLimit * 1024;

            try
            {
                WaitForBusinessSoftwareToClose();
                
                if (isLargeFile)
                {
                    Console.WriteLine($"Waiting to transfer large file: {file}");
                    _largeFileSemaphore.Wait(); // Assure un seul fichier volumineux à la fois
                }

                waitHandle.WaitOne();

                DateTime start = DateTime.Now;
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= fileInfo.Length;

                OnStateUpdated(new StateEvent("Differential Backup", "Active", remainingFiles, remainingSize, remainingFiles, remainingSize, file, destFile));
                OnProgress(new ProgressEvent(totalSize, remainingSize));
                TransferEvent transferEvent = new TransferEvent(_configuration, duration, fileInfo, new FileInfo(destFile));
                OnTransfer(transferEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error copying file {file}: {e.Message}");
                OnTransfer(new TransferEvent(_configuration, new TimeSpan(-1), fileInfo, new FileInfo(destFile)));
            }
            finally
            {
                if (isLargeFile)
                {
                    _largeFileSemaphore.Release();
                }
            }
        }

        /// <summary>
        /// Checks if a file should be backed up.
        /// </summary>
        private bool ShouldBackupFile(string sourceFile, string destFile)
        {
            if (!File.Exists(destFile))
            {
                Console.WriteLine($"[DIFF] Nouveau fichier détecté : {sourceFile}");
                return true; 
            }

            FileInfo sourceInfo = new FileInfo(sourceFile);
            FileInfo destInfo = new FileInfo(destFile);

            if (sourceInfo.LastWriteTime > destInfo.LastWriteTime)
            {
                Console.WriteLine($"[DIFF] Fichier modifié détecté : {sourceFile}");
                return true; 
            }

            Console.WriteLine($"[DIFF] Pas de modification pour : {sourceFile}");
            return false; 
        }
        
        private bool PreviousSaveExists()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(_configuration);
            return cached != null;
        }

        private string PreviousSavePath()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(_configuration);
            return cached!.Backups.OrderByDescending(b => b.Date).First().DirectoryName;
        }

        private void UpdateCache(string uniqueDestinationPath)
        {
            _cacheRepository.AddBackup(_configuration, DateTime.Now, uniqueDestinationPath);
        }
    }
}
