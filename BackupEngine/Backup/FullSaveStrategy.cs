using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackupEngine.State;
using BackupEngine.Log;
using BackupEngine.Settings;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Implements the full backup strategy.
    /// </summary>
    public class FullSaveStrategy : SaveStrategy
    {
        private SettingsRepository _settingsRepository = new SettingsRepository();
        private readonly int _koLimit = 1024;
        private readonly SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Mutex name to ensure only one CryptoSoft instance runs at a time.
        /// </summary>
        private static readonly string _mutexName = "Global\\CryptoSoft_Mutex";
        /// <summary>
        /// Queue to store files that require encryption.
        /// </summary>
        private readonly ConcurrentQueue<(string, string)> _cryptoQueue = new ConcurrentQueue<(string, string)>();
        /// <summary>
        /// Task responsible for processing the encryption queue.
        /// </summary>
        private Task _cryptoTask;
        /// <summary>
        /// Initializes a new instance of the FullSaveStrategy class.
        /// </summary>
        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Executes the full backup process.
        /// </summary>
        public override void Save(string uniqueDestinationPath)
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

            OnStateUpdated(new StateEvent("Full Backup", "Active", totalFiles, totalSize, remainingFiles, remainingSize, "", ""));
            List<Task> tasks = new List<Task>();
            WaitForBusinessSoftwareToClose();
            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

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

            OnStateUpdated(new StateEvent("Full Backup", "Completed", totalFiles, totalSize, 0, 0, "", ""));
            Console.WriteLine($"Full backup completed in: {uniqueDestinationPath}");
        }

        /// <summary>
        /// Transfers a file and updates the backup state.
        /// </summary>
        private void TransferFile(string file, string destFile, ref int remainingFiles, ref long remainingSize)
        {
            FileInfo fileInfo = new FileInfo(file);
            bool isLargeFile = fileInfo.Length > _koLimit * 1024;

            try
            {
                // 🔴 Attente ici pour empêcher la copie tant qu'un logiciel métier est ouvert
                WaitForBusinessSoftwareToClose();

                if (isLargeFile)
                {
                    Console.WriteLine($"Waiting to transfer large file: {file}");
                    _largeFileSemaphore.Wait(); // Assure un seul fichier volumineux à la fois
                }

                DateTime start = DateTime.Now;
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= fileInfo.Length;

                OnStateUpdated(new StateEvent("Full Backup", "Active", remainingFiles, remainingSize, remainingFiles, remainingSize, file, destFile));

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
                    _largeFileSemaphore.Release(); // Libère le sémaphore après transfert
                }
            }
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
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Determines if a file requires encryption based on its extension.
        /// </summary>
        private bool RequiresEncryption(string file)
        {
            string extension = Path.GetExtension(file);
            if (_configuration.ExtensionsToSave == null)
            {
                return false;
            }
            return _configuration.ExtensionsToSave.Contains(extension);
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
    }
}
