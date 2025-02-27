using BackupEngine.Backup;
using static System.Reflection.Metadata.BlobBuilder;

namespace BackupEngine.Job
{
    /// <summary>
    /// Represents a backup job that manages the execution of a backup operation.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// The backup configuration associated with this job.
        /// </summary>
        public BackupConfiguration Configuration { get; }

        /// <summary>
        /// The file manager responsible for handling file operations during the backup.
        /// </summary>
        private FileManager FileManager { get; }

        /// <summary>
        /// Cancellation token used to stop the job if needed.
        /// </summary>
        private readonly CancellationToken _ct;

        /// <summary>
        /// The current progress percentage of the backup process.
        /// </summary>
        public double Progress = 0;

        /// <summary>
        /// Event triggered when the progress of the job changes.
        /// </summary>
        public event EventHandler<double> ProgressChanged;

        /// <summary>
        /// Event handle used to manage pause and resume functionality.
        /// </summary>
        private readonly EventWaitHandle _waitHandle;
        /// <summary>
        /// Initializes a new instance of the Job class with the specified backup configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration for this job.</param>
        /// <param name="Token">The cancellation token to allow stopping the job.</param>
        /// <param name="waitHandle">The event handle used for pausing and resuming.</param>
        public Job(BackupConfiguration configuration, CancellationToken Token, EventWaitHandle waitHandle)
        {
            Configuration = configuration;
            _ct = Token;
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy(Configuration, _ct));
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
        /// Executes the backup process while periodically checking for cancellation requests.
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