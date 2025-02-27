using BackupEngine.Backup;

namespace BackupEngine.Job
{
    /// <summary>
    /// The Job class is responsible for managing the backup.
    /// It configures the type of backup to use (full or differential) and initiates the backup via the FileManager.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Private properties:
        /// Configuration: Contains the backup configuration, specifying details like the backup type.
        /// FileManager: Manages the backup based on the configuration.
        /// CryptStrategy: Encryption strategy used for secure file transfer. Instantiated by default.
        /// </summary>
        public BackupConfiguration Configuration { get; }
        private FileManager FileManager { get; }
        private readonly CancellationToken _ct;
        public double Progress = 0;
        public event EventHandler<double> ProgressChanged;
        private readonly EventWaitHandle _waitHandle;
        
        /// <summary>
        /// Constructor of the Job class. Accepts a backup configuration.
        /// Depending on the backup type specified in the configuration, it initializes the FileManager with the appropriate strategy.
        /// </summary>
        public Job(BackupConfiguration configuration, CancellationToken Token, EventWaitHandle waitHandle)
        {
            Configuration = configuration;
            _ct = Token;
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy(Configuration));
                    break;
                case BackupType.Differential:
                    FileManager = new FileManager(new DifferentialSaveStrategy(Configuration));
                    break;
                default:
                    throw new Exception("Invalid backup type");
            }

            _waitHandle = waitHandle;
        }

        /// <summary>
        /// Public method Run() that triggers the backup via the FileManager.
        /// </summary>
        public void Run()
        {
            // Periodically check for cancellation
            if (_ct.IsCancellationRequested)
            {
                return;
            }
            FileManager.SubscribeProgress((sender, e) =>
            {
                long total = e.TotalSize - e.RemainingSize;
                double progress1 = ((double) total / e.TotalSize);
                double progress = progress1 * 100;
                ProgressChanged?.Invoke(this, progress);
            });
            FileManager.Save(Configuration, _waitHandle);
        }
    }
}