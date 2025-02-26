using BackupEngine.Backup;

namespace BackupEngine.Job
{
    public class Job
    {
        public BackupConfiguration Configuration { get; }
        private FileManager FileManager { get; }
        private readonly CancellationToken _ct;
        public int Progress = 0;
        public event EventHandler<int> ProgressChanged;

        public Job(BackupConfiguration configuration, CancellationToken Token)
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
                Progress = (int)(e.RemainingSize / e.TotalSize);
                ProgressChanged?.Invoke(this, Progress);
            });
            FileManager.Save(Configuration);
        }
    }
}