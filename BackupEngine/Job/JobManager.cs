using System.Diagnostics;

namespace BackupEngine.Job
{
    /// <summary>
    /// Manages the creation, execution, and control of backup jobs.
    /// </summary>
    public class JobManager
    {
        /// <summary>
        /// Dictionary storing active jobs and their associated threads.
        /// </summary>
        private Dictionary<Job, Thread> _jobs { get; } = new();

        /// <summary>
        /// Dictionary storing cancellation tokens for each active job.
        /// </summary>
        private Dictionary<Job, CancellationTokenSource> _cancellationTokens = new();

        /// <summary>
        /// Dictionary storing event wait handles to manage pausing and resuming jobs.
        /// </summary>
        private Dictionary<Job, EventWaitHandle> _waitHandles = new();

        /// <summary>
        /// Launches a new backup job based on the given configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration for the job.</param>
        /// <returns>The created and started job instance.</returns>

        public Job LaunchBackup(BackupConfiguration configuration)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            EventWaitHandle waitHandle = new ManualResetEvent(initialState: true);
            Job job = new Job(configuration, cts.Token, waitHandle); // Passe la source
            Thread thread = new Thread(() => job.Run());
            
            _jobs.Add(job, thread);
            _cancellationTokens.Add(job, cts);
            _waitHandles.Add(job, waitHandle);

            thread.Start();
            return job;
        }
       
        /// <summary>
        /// Launches multiple backup jobs based on a list of configurations.
        /// </summary>
        /// <param name="configurations">The list of backup configurations.</param>
        /// <returns>A list of started job instances.</returns>
        public List<Job> LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = new List<Job>();
            foreach (BackupConfiguration configuration in configurations)
            {
                jobs.Add(LaunchBackup(configuration));
            }
            return jobs;
        }
        /// <summary>
        /// Pauses a running backup job.
        /// </summary>
        /// <param name="job">The job to be paused.</param>
        public void PauseJob(Job job)
        {
            // Pause the task
            _waitHandles[job].Reset();
        }
        /// <summary>
        /// Resumes a paused backup job.
        /// </summary>
        /// <param name="job">The job to be resumed.</param>
        public void ResumeJob(Job job)
        {
            // Resume the task
            _waitHandles[job].Set();
        }
        /// <summary>
        /// Cancels and stops a running backup job.
        /// </summary>
        /// <param name="job">The job to be cancelled.</param>
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
