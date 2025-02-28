using BackupEngine.Progress;
using BackupEngine.State;
using BackupEngine.Log;
using System.Collections.Concurrent;

namespace BackupEngine.Backup
{
    public class FullSaveStrategy : SaveStrategy
    {
        private readonly CancellationToken _cancellationToken;
        private readonly SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);
        private readonly ConcurrentQueue<(string, string)> _cryptoQueue = new ConcurrentQueue<(string, string)>();
        private Task? _cryptoTask;

        public FullSaveStrategy(BackupConfiguration configuration, CancellationToken cancellationToken)
            : base(configuration)
        {
            _cancellationToken = cancellationToken;
        }

        public override void Save(string uniqueDestinationPath, EventWaitHandle waitHandle)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Sauvegarde annulée avant démarrage.");
                return;
            }

            // Priorisation des extensions
            HashSet<string> priorityExtensions = _settingsRepository.GetExtensionPriority();
            TransferStrategy = Configuration.ExtensionsToSave != null
                ? new CryptStrategy(Configuration.ExtensionsToSave, priorityExtensions, _cancellationToken)
                : new CopyStrategy();

            string sourcePath = Configuration.SourcePath.GetAbsolutePath();
            if (!Directory.Exists(sourcePath))
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");

            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            OnStateUpdated(new StateEvent("Full Backup", "Active", totalFiles, totalSize, remainingFiles, remainingSize, "", ""));
            OnProgress(new ProgressEvent(totalSize, remainingSize));

            List<Task> tasks = new List<Task>();
            WaitForBusinessSoftwareToClose();

            foreach (string file in files)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Sauvegarde annulée pendant le traitement.");
                    return;
                }

                waitHandle.WaitOne();
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile) ?? throw new InvalidOperationException());

                if (RequiresEncryption(file))
                {
                    _cryptoQueue.Enqueue((file, destFile));
                }
                else
                {
                    tasks.Add(Task.Run(() => TransferFile(file, destFile, ref totalSize, ref remainingFiles, ref remainingSize, waitHandle)));
                }
            }

            _cryptoTask = Task.Run(() => { WaitForBusinessSoftwareToClose(); ProcessCryptoQueue(); });

            Task.WhenAll(tasks).Wait();
            _cryptoTask.Wait();

            if (_cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Sauvegarde annulée avant achèvement.");
                return;
            }

            OnStateUpdated(new StateEvent("Full Backup", "Completed", totalFiles, totalSize, 0, 0, "", ""));
            OnProgress(new ProgressEvent(totalSize, 0));
            Console.WriteLine($"Sauvegarde complète terminée dans: {uniqueDestinationPath}");
        }

        private void TransferFile(string file, string destFile, ref long totalSize, ref int remainingFiles, ref long remainingSize, EventWaitHandle waitHandle)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Sauvegarde annulée.");
                return;
            }

            FileInfo fileInfo = new FileInfo(file);
            bool isLargeFile = fileInfo.Length > _koLimit * 1024;

            try
            {
                WaitForBusinessSoftwareToClose();

                if (isLargeFile)
                {
                    Console.WriteLine($"Attente pour transférer un grand fichier: {file}");
                    _largeFileSemaphore.Wait();
                }

                waitHandle.WaitOne();

                DateTime start = DateTime.Now;
                TransferStrategy.TransferFile(file, destFile);
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                remainingFiles--;
                remainingSize -= fileInfo.Length;

                OnStateUpdated(new StateEvent("Full Backup", "Active", remainingFiles, remainingSize, remainingFiles, remainingSize, file, destFile));
                OnProgress(new ProgressEvent(totalSize, remainingSize));

                TransferEvent transferEvent = new TransferEvent(Configuration, duration, fileInfo, new FileInfo(destFile));
                OnTransfer(transferEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur de copie du fichier {file}: {e.Message}");
                OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), fileInfo, new FileInfo(destFile)));
            }
            finally
            {
                if (isLargeFile)
                {
                    _largeFileSemaphore.Release();
                }
            }
        }

        private void ProcessCryptoQueue()
        {
            while (_cryptoQueue.TryDequeue(out var filePair))
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cryptage annulé.");
                    return;
                }

                string source = filePair.Item1;
                string destination = filePair.Item2;
                TransferStrategy.TransferFile(source, destination);
            }
        }
    }
}
