namespace BackupEngine.Job
{
    internal class Job
    {
        private BackupConfiguration Configuration { get; set; }

        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Run()
        {

        }
    }
}