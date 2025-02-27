using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Settings;
using BackupEngine.State;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Abstract class SaveStrategy that defines the base for backup strategies (full, differential, etc.).
    /// It includes events to report the state of the backup and file transfers.
    /// </summary>
    public abstract class SaveStrategy(BackupConfiguration configuration)
    {
        /// <summary>
        /// Events to notify transfer and state information during the backup.
        /// </summary>
        public event EventHandler<TransferEvent> Transfer;
        public event EventHandler<StateEvent> StateUpdated;
        public event EventHandler<ProgressEvent> Progress;
        protected readonly BackupConfiguration Configuration = configuration;
        public ITransferStrategy TransferStrategy;
        public SettingsRepository _settingsRepository = new SettingsRepository();
        protected readonly int _koLimit = 1024;
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

        public abstract void Save(string uniqueDestinationPath, EventWaitHandle waitHandle);

        /// <summary>
        /// Protected method to notify a transfer event.
        /// </summary>
        protected void OnTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }

        /// <summary>
        /// Protected method to notify a state update event.
        /// </summary>
        protected void OnStateUpdated(StateEvent state)
        {
            StateUpdated?.Invoke(this, state);
        }

        protected void OnProgress(ProgressEvent progress)
        {
            Progress?.Invoke(this, progress);
        }

        /// <summary>
        /// Processes the queue of files that need encryption.
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
        /// Determines if a file requires encryption based on its extension.
        /// </summary>
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