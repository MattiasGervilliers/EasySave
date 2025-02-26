using System.Diagnostics;

namespace BackupEngine.Job
{
    public class JobManager
    {
        private Dictionary<Job, Thread> _jobs { get; } = new();
        private Dictionary<Job, CancellationTokenSource> _cancellationTokens = new();
        private Dictionary<Job, EventWaitHandle> _waitHandles = new();

        public Job LaunchBackup(BackupConfiguration configuration)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            EventWaitHandle waitHandle = new ManualResetEvent(initialState: true);
            Job job = new Job(configuration, cts.Token, waitHandle);
            Thread thread = new Thread(() => job.Run());
            
            _jobs.Add(job, thread);
            _cancellationTokens.Add(job, cts);
            _waitHandles.Add(job, waitHandle);

            thread.Start();
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
            _waitHandles[job].Reset();
        }

        public void ResumeJob(Job job)
        {
            // Resume the task
            _waitHandles[job].Set();
        }

        public void CancelJob(Job job)
        {
            if (_jobs.ContainsKey(job) && _cancellationTokens.ContainsKey(job))
            {
                _cancellationTokens[job].Cancel(); // Request cancellation
                _jobs.Remove(job);
                _cancellationTokens.Remove(job);
                _waitHandles.Remove(job);
            }
        }
    }
}
