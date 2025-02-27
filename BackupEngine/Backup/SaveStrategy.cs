using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Settings;
using BackupEngine.State;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BackupEngine.Backup
{
    public abstract class SaveStrategy(BackupConfiguration configuration)
    {
        /// <summary>
        /// Event triggered when a file transfer occurs.
        /// </summary>
        public event EventHandler<TransferEvent> Transfer;
        /// <summary>
        /// Event triggered when the state of the backup process updates.
        /// </summary>
        public event EventHandler<StateEvent> StateUpdated;
        /// <summary>
        /// Event triggered to report progress updates during the backup process.
        /// </summary>
        public event EventHandler<ProgressEvent> Progress;
        /// <summary>
        /// The backup configuration associated with the save strategy.
        /// </summary>
        protected readonly BackupConfiguration Configuration = configuration;
        /// <summary>
        /// The file transfer strategy used to perform the backup operation.
        /// </summary>
        public ITransferStrategy TransferStrategy;
        /// <summary>
        /// Repository managing application settings.
        /// </summary>
        public SettingsRepository _settingsRepository = new SettingsRepository();
        /// <summary>
        /// File size limit (in KB) to determine special handling for large files.
        /// </summary>
        protected readonly int _koLimit = 1024;
        /// <summary>
        /// Semaphore to ensure that only one large file is processed at a time.
        /// </summary>
        protected readonly SemaphoreSlim _largeFileSemaphore = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Mutex name to ensure only one CryptoSoft instance runs at a time.
        /// </summary>
        protected static readonly string _mutexName = "Global\\CryptoSoft_Mutex";
        /// <summary>
        /// Queue to store files that require encryption.
        /// </summary>
        protected readonly ConcurrentQueue<(string, string)> _cryptoQueue = new ConcurrentQueue<(string, string)>();
        /// <summary>
        /// Task responsible for processing the encryption queue.
        /// </summary>
        protected Task _cryptoTask;
        /// <summary>
        /// Abstract method that executes the backup process.
        /// </summary>
        /// <param name="uniqueDestinationPath">The destination path for the backup.</param>
        /// <param name="waitHandle">Event handle for managing pause and resume functionality.</param>
        public abstract void Save(string uniqueDestinationPath, EventWaitHandle waitHandle);
        /// <summary>
        /// Triggers the Transfer event to notify about a file transfer.
        /// </summary>
        /// <param name="e">Transfer event details.</param>
        protected void OnTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }
        /// <summary>
        /// Triggers the StateUpdated event to notify about a change in backup state.
        /// </summary>
        /// <param name="state">State event details.</param>
        protected void OnStateUpdated(StateEvent state)
        {
            StateUpdated?.Invoke(this, state);
        }
        /// <summary>
        /// Triggers the Progress event to provide an update on the backup progress.
        /// </summary>
        /// <param name="progress">Progress event details.</param>
        protected void OnProgress(ProgressEvent progress)
        {
            Progress?.Invoke(this, progress);
        }

        /// <summary>
        /// Processes the queue of files that need encryption, ensuring only one CryptoSoft instance runs at a time.
        /// </summary>
        public void ProcessCryptoQueue()
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

        /// <summary>
        /// Determines whether a file requires encryption based on its extension.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <returns>True if encryption is required, otherwise false.</returns>
        protected bool RequiresEncryption(string file)
        {
            string extension = Path.GetExtension(file);
            if (Configuration.ExtensionsToSave == null)
            {
                return false;
            }
            return Configuration.ExtensionsToSave.Contains(extension);
        }

        /// <summary>
        /// Waits for business software to close before proceeding with the backup.
        /// </summary>
        protected void WaitForBusinessSoftwareToClose()
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
        /// <param name="businessApps">List of business applications to check.</param>
        /// <returns>True if a business application is running, otherwise false.</returns>
        protected bool IsBusinessSoftwareRunning(List<string> businessApps)
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