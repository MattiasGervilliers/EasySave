﻿using BackupEngine.Backup;

namespace BackupEngine.Job
{
    public class Job
    {
        private BackupConfiguration Configuration { get; set; }
        private FileManager FileManager { get; set; }
        private CryptStrategy _cryptStrategy = new CryptStrategy("test",null);

        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;
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
            FileManager.Save(Configuration);
        }
    }
}