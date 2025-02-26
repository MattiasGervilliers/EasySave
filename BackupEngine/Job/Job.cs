using BackupEngine.Backup;

namespace BackupEngine.Job
{
    public class Job
    {
        public BackupConfiguration Configuration { get; }
        private FileManager FileManager { get; }
        private readonly CancellationToken _ct;
        public double Progress = 0;
        public event EventHandler<double> ProgressChanged;
        private readonly EventWaitHandle _waitHandle;

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