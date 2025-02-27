using BackupEngine;
using BackupEngine.Job;

namespace EasySaveGUI.Models
{
    internal class BackupModel
    {
        /// <summary>
        /// Manages the execution and control of backup jobs.
        /// </summary>
        private readonly JobManager _jobManager = new JobManager();

        /// <summary>
        /// Dictionary storing active backup jobs associated with their configurations.
        /// </summary>
        private readonly Dictionary<BackupConfiguration, Job> _jobs = [];

        /// <summary>
        /// Event triggered to update the progress of backup operations.
        /// </summary>
        public event Action<BackupConfiguration, double>? ProgressUpdated;

        /// <summary>
        /// Launches backup operations for multiple configurations.
        /// </summary>
        /// <param name="configurations">The list of backup configurations to execute.</param>
        public void LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = _jobManager.LaunchBackup(configurations);
           
            for (int i = 0; i < configurations.Count; ++i)
            {
                _jobs.Add(configurations[i], jobs[i]);
                BackupConfiguration conf = configurations[i];
                jobs[i].ProgressChanged += (sender, progress) => OnProgressChange(conf, progress);
            }
        }
        /// <summary>
        /// Handles progress updates for a backup operation.
        /// </summary>
        /// <param name="configuration">The backup configuration being updated.</param>
        /// <param name="progress">The current progress percentage.</param>
        private void OnProgressChange(BackupConfiguration configuration, double progress)
        {
            if (progress >= 100)
            {
                _jobs.Remove(configuration);
            }
            ProgressUpdated?.Invoke(configuration, progress);
        }
        /// <summary>
        /// Launches a backup operation for a single configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to execute.</param>
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
        /// <summary>
        /// Pauses an ongoing backup associated with a specific configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to pause.</param>
        public void PauseBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.PauseJob(job);
        }
        /// <summary>
        /// Resumes a paused backup associated with a specific configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to resume.</param>
        public void ResumeBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.ResumeJob(job);
        }
        /// <summary>
        /// Cancels and removes a backup operation for a given configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to remove.</param>
        public void RemoveBackup(BackupConfiguration configuration)
        {
            Job job = _jobs[configuration];
            _jobManager.CancelJob(job);
            _jobs.Remove(configuration);
        }
    }
}
