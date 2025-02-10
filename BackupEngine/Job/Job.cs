using BackupEngine.Backup;

namespace BackupEngine.Job
{
    public class Job
    {
        private BackupConfiguration Configuration { get; set; }
        private FileManager FileManager { get; set; }

        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy(Configuration));
                    break;
                case BackupType.Incremental:
                    FileManager = new FileManager(new IncrementalSaveStrategy(Configuration));
                    break;
                default:
                    throw new Exception("Invalid backup type");
            }
        }

        public void Run()
        {
            FileManager.Save(Configuration);
        }
    }
}