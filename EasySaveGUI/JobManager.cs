using System.Collections.Generic;
using System.Threading;

namespace BackupEngine.Job
{
    /// <summary>
    /// Class responsible for managing backup jobs.
    /// It allows launching backup jobs, each in a separate thread.
    /// </summary>
    public class JobManager
    {
        // List that maintains the threads of the currently running jobs.
        private List<Thread> JobsThreads { get; set; }

        /// <summary>
        /// Constructor for the JobManager class.
        /// Initializes the list of job threads.
        /// </summary>
        public JobManager()
        {
            JobsThreads = new List<Thread>();  // Initializes the list of job threads
        }

        /// <summary>
        /// Launches a backup job in a separate thread using the given configuration.
        /// </summary>
        /// <param name="configuration">The configuration of the backup to execute.</param>
        /// <returns>The launched backup job.</returns>
        public Job LaunchBackup(BackupConfiguration configuration)
        {
            Job job = new Job(configuration);  // Creates a new job with the provided configuration
            Thread thread = new Thread(() => job.Run());  // Creates a thread to execute the job
            JobsThreads.Add(thread);  // Adds the thread to the list of active threads
            thread.Start();  // Starts the thread
            return job;  // Returns the launched job
        }

        /// <summary>
        /// Launches multiple backup jobs in separate threads, using a list of configurations.
        /// </summary>
        /// <param name="configurations">The list of backup configurations to execute.</param>
        /// <returns>The list of launched backup jobs.</returns>
        public List<Job> LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = new List<Job>();  // Creates a new list to store the launched jobs
            foreach (BackupConfiguration configuration in configurations)
            {
                jobs.Add(LaunchBackup(configuration));  // Launches each job and adds it to the list
            }
            return jobs;  // Returns the list of launched jobs
        }

        /// <summary>
        /// Private method to stop a running backup job.
        /// This method is currently unimplemented.
        /// </summary>
        /// <param name="job">The job to stop.</param>
        void StopJob(Job job)
        {
            // This method could contain the logic to stop a running job,
            // but it is currently empty and unimplemented.
        }
    }
}
