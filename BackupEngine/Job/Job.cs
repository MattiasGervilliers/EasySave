using BackupEngine.Backup;
using Newtonsoft.Json.Linq;

namespace BackupEngine.Job
{
    public class Job
    {
        private BackupConfiguration Configuration { get; }
        private FileManager FileManager { get; }
        private readonly CancellationToken _ct;

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

            FileManager.Save(Configuration);
        }
    }
}