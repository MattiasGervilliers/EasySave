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
        private BackupConfiguration Configuration { get; set; }
        private FileManager FileManager { get; set; }
        private CryptStrategy _cryptStrategy = new CryptStrategy();

        /// <summary>
        /// Constructor of the Job class. Accepts a backup configuration.
        /// Depending on the backup type specified in the configuration, it initializes the FileManager with the appropriate strategy.
        /// </summary>
        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;

            /// <summary>
            /// Depending on the backup type, instantiate a FileManager with the correct backup strategy.
            /// </summary>
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy(Configuration));
                    break;
                case BackupType.Differential:
                    FileManager = new FileManager(new DifferentialSaveStrategy(Configuration));
                    break;
                default:
                    /// <summary>
                    /// If the backup type is invalid, an exception is thrown.
                    /// </summary>
                    throw new Exception("Invalid backup type");
            }
        }

        /// <summary>
        /// Public method Run() that triggers the backup via the FileManager.
        /// </summary>
        public void Run()
        {
            /// <summary>
            /// Calls the Save method of the FileManager to perform the backup with the provided configuration.
            /// </summary>
            FileManager.Save(Configuration);
        }
    }
}
