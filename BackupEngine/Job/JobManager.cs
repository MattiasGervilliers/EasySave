namespace BackupEngine.Job
{
    public class JobManager
    {
        private Dictionary<Job, Task> _jobs { get; } = new();
        private Dictionary<Job, CancellationTokenSource> _cancellationTokens = new();

        public Job LaunchBackup(BackupConfiguration configuration)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Job job = new Job(configuration, cts.Token);
            Task task = new Task(() => job.Run());
            
            _jobs.Add(job, task);
            _cancellationTokens.Add(job, cts);

            task.Start();
            return job;
        }

        public List<Job> LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = new List<Job>();
            foreach (BackupConfiguration configuration in configurations)
            {
                jobs.Add(LaunchBackup(configuration));
            }
            return jobs;
        }

        public void PauseJob(Job job)
        {
            // Pause the task
            _jobs[job]?.Wait();
        }

        public void ResumeJob(Job job)
        {
            // Resume the task
            _jobs[job]?.Start();
        }

        public void CancelJob(Job job)
        {
            if (_jobs.ContainsKey(job) && _cancellationTokens.ContainsKey(job))
            {
                _cancellationTokens[job].Cancel(); // Request cancellation
                _jobs[job].Wait(); // Wait for the task to complete
                _jobs.Remove(job);
                _cancellationTokens.Remove(job);
            }
        }
    }
}
