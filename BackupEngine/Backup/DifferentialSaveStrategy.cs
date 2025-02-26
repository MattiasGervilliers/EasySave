using BackupEngine.State;
using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Settings;
using System.Diagnostics;
using System.Collections.Concurrent;
using BackupEngine.Cache;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Implements the differential backup strategy.
    /// </summary>
    public class DifferentialSaveStrategy : SaveStrategy
    {
        private SettingsRepository _settingsRepository = new SettingsRepository();
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();
        private static readonly string _mutexName = "Global\\CryptoSoft_Mutex";
        private readonly ConcurrentQueue<(string, string)> _cryptoQueue = new ConcurrentQueue<(string, string)>();
        private Task _cryptoTask;

        /// <summary>
        /// Initializes a new instance of the DifferentialSaveStrategy class.
        /// </summary>
        public DifferentialSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Executes the differential backup process.
        /// </summary>
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
            fullSaveStrategy.Progress += (sender, e) => OnProgress(e);
            fullSaveStrategy.Save(uniqueDestinationPath);
        }

        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath)
        {
            if (Configuration.ExtensionsToSave != null)
            {
                TransferStrategy = new CryptStrategy(Configuration.ExtensionsToSave, _settingsRepository.GetExtensionPriority());
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
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                if (!ShouldBackupFile(file, destFile)) 
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
                        TransferFile(file, destFile, ref remainingFiles, ref remainingSize);
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
        private void TransferFile(string file, string destFile, ref int remainingFiles, ref long remainingSize)
        {
            FileInfo fileInfo = new FileInfo(file);

            try
            {
                WaitForBusinessSoftwareToClose();
                DateTime start = DateTime.Now;
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= fileInfo.Length;

                OnStateUpdated(new StateEvent("Differential Backup", "Active", remainingFiles, remainingSize, remainingFiles, remainingSize, file, destFile));
                OnProgress(new ProgressEvent(totalSize, remainingSize));
                
                TransferEvent transferEvent = new TransferEvent(Configuration, duration, fileInfo, new FileInfo(destFile));
                OnTransfer(transferEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error copying file {file}: {e.Message}");
                OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), fileInfo, new FileInfo(destFile)));
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

        /// <summary>
        /// Determines if a file requires encryption based on its extension.
        /// </summary>
        private bool RequiresEncryption(string file)
        {
            string extension = Path.GetExtension(file);
            return Configuration.ExtensionsToSave.Contains(extension);
        }

        /// <summary>
        /// Processes the queue of files that need encryption.
        /// </summary>
        private void ProcessCryptoQueue()
        {
            using (Mutex mutex = new Mutex(false, _mutexName))
            {
                while (!_cryptoQueue.IsEmpty)
                {
                    if (_cryptoQueue.TryDequeue(out var filePair))
                    {
                        string source = filePair.Item1;
                        string destination = filePair.Item2;

                        if (!mutex.WaitOne(0, false))
                        {
                            Console.WriteLine("CryptoSoft est déjà en cours d'exécution");
                            mutex.WaitOne();
                        }

                        try
                        {
                            TransferStrategy.TransferFile(source, destination);
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                }
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

        /// <summary>
        /// Waits for business software to close before proceeding with the backup.
        /// </summary>
        private void WaitForBusinessSoftwareToClose()
        {
            List<string> businessApps = _settingsRepository.GetBusinessSoftwareList();
            while (IsBusinessSoftwareRunning(businessApps))
            {
                Console.WriteLine("Un logiciel métier est en cours d'exécution");
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Checks if any business software is currently running.
        /// </summary>
        private bool IsBusinessSoftwareRunning(List<string> businessApps)
        {
            foreach (var process in Process.GetProcesses())
            {
                if (businessApps.Contains(process.ProcessName, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Logiciel métier détecté : {process.ProcessName}");
                    return true;
                }
            }
            return false;
        }
    }
}
