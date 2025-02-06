using BackupEngine.Backup;

namespace BackupEngine.Job
{
    internal class Job
    {
        private BackupConfiguration Configuration { get; set; }
        private FileManager FileManager { get; set; }

        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy());
                    break;
                case BackupType.Incremental:
                    FileManager = new FileManager(new IncrementalSaveStrategy());
                    break;
            }

            if (FileManager == null)
            {
                throw new System.Exception("Invalid backup type");
            }
        }

        public void Run()
        {
            FileManager.Save(Configuration.SourcePath.GetAbsolutePath(), Configuration.DestinationPath.GetAbsolutePath());
        }
    }
}