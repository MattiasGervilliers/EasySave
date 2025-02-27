using BackupEngine.Progress;
using BackupEngine.State;
using BackupEngine.Log;
using System.Diagnostics;
//using BackupEngine.Remote;
using BackupEngine.Job;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Implements the full backup strategy.
    /// </summary>
    public class FullSaveStrategy : SaveStrategy
    {
        
        /// <summary>
        /// Initializes a new instance of the FullSaveStrategy class.
        /// </summary>
        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Executes the full backup process.
        /// </summary>
        public override void Save(string uniqueDestinationPath, EventWaitHandle waitHandle)
        {
            if (Configuration.ExtensionsToSave != null)
            {
                //StartServer();
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

            OnStateUpdated(new StateEvent("Full Backup", "Active", totalFiles, totalSize, remainingFiles, remainingSize, "", ""));
            OnProgress(new ProgressEvent(
                totalSize,
                remainingSize
            ));
            
            List<Task> tasks = new List<Task>();
            WaitForBusinessSoftwareToClose();
            
            foreach (string file in files)
            {
                waitHandle.WaitOne();
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
                        Console.WriteLine("1111111111111"+file+destFile);
                        TransferFile(file, destFile, ref totalSize, ref remainingFiles, ref remainingSize, ref waitHandle);
                    }));
                    Console.WriteLine("ZZZZZZZZZZZZZZZZZZ" + file);

                }
            }
            
            _cryptoTask = Task.Run(() =>
            {
                    Console.WriteLine("EEEEEEEEEEEEEEEEEE");
                WaitForBusinessSoftwareToClose(); 
                ProcessCryptoQueue();
            });

            Task.WhenAll(tasks).Wait();
            _cryptoTask.Wait();

            OnStateUpdated(new StateEvent("Full Backup", "Completed", totalFiles, totalSize, 0, 0, "", ""));
            OnProgress(new ProgressEvent(totalSize,0));
            Console.WriteLine($"Full backup completed in: {uniqueDestinationPath}");
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

                // Check for pausing
                waitHandle.WaitOne();

                DateTime start = DateTime.Now;
                Console.WriteLine("000000000000000000000000000transfer de "+file + "->" + destFile);
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= fileInfo.Length;

                OnStateUpdated(new StateEvent("Full Backup", "Active", remainingFiles, remainingSize, remainingFiles, remainingSize, file, destFile));
                OnProgress(new ProgressEvent(totalSize,remainingSize));
                
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
                    _largeFileSemaphore.Release(); // Libère le sémaphore après transfert
                }
            }
        }
                /*
        public static void StartServer()
        {
            _server.Start();
        }
        private JobManager JobManager = new JobManager();
        private static void HandleRemoteCommand(string command)
        {
            if (command == "pause")
            {
                JobManager.PauseJob();
                _isPaused = true;
                Console.WriteLine("[Remote] Pause de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isPaused = true;
                }
            }
            else if (command == "resume")
            {
                _isPaused = false;

                Console.WriteLine("[Remote] Reprise de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isPaused = false;
                    Monitor.PulseAll(_pauseLock); // Réveille les threads en pause
                }
            }
            else if (command == "stop")
            {
                Console.WriteLine("[Remote] Arrêt de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isStopped = true;
                    Monitor.PulseAll(_pauseLock);
                }
            }
        }
                 * */
    }
}
