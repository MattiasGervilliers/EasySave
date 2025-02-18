using BackupEngine;
using BackupEngine.Job;

namespace EasySaveGUI.Models
{
    internal class BackupModel
    {
        private readonly JobManager _jobManager = new JobManager();

        public void LaunchBackup(List<BackupConfiguration> configurations)
        {
            _jobManager.LaunchBackup(configurations);
        }

        public void LaunchBackup(BackupConfiguration configuration)
        {
            _jobManager.LaunchBackup(configuration);
        }
    }
}
