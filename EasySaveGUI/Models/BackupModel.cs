using BackupEngine;
using BackupEngine.Job;

namespace EasySaveGUI.Models
{
    internal class BackupModel
    {
        private readonly JobManager _jobManager = new JobManager();

        private readonly Dictionary<BackupConfiguration, Job> _jobs = [];
        public event Action<BackupConfiguration, int>? ProgressUpdated;

        public void LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = _jobManager.LaunchBackup(configurations);
           
            for (int i = 0; i < configurations.Count; i++)
            {
                _jobs.Add(configurations[i], jobs[i]);
                jobs[i].ProgressChanged += (sender, progress) => OnProgressChange(configurations[i], progress);
            }
        }

        private void OnProgressChange(BackupConfiguration configuration, int progress)
        {
            ProgressUpdated?.Invoke(configuration, progress);
        }

        public void LaunchBackup(BackupConfiguration configuration)
        {
            if (_jobs.ContainsKey(configuration))
            {
                return;
            }
            Job job = _jobManager.LaunchBackup(configuration);
            _jobs.Add(configuration, job);
            job.ProgressChanged += (sender, progress) => OnProgressChange(configuration, progress);
        }

        public void PauseBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.PauseJob(job);
        }

        public void ResumeBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.ResumeJob(job);
        }

        public void RemoveBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.CancelJob(job);
            _jobs.Remove(configuration);
        }
    }
}
