using System.Collections.Generic;
using System.Threading;

namespace BackupEngine.Job
{
    public class JobManager
    {
        private List<Thread> JobsThreads { get; set; }

        public JobManager()
        {
            JobsThreads = new List<Thread>();
        }

        public Job LaunchBackup(BackupConfiguration configuration)
        {
            Job job = new Job(configuration);
            Thread thread = new Thread(() => job.Run());
            JobsThreads.Add(thread);
            thread.Start();
            return job;
        }

        List<Job> LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = new List<Job>();
            foreach (BackupConfiguration configuration in configurations)
            {
                jobs.Add(LaunchBackup(configuration));
            }
            return jobs;
        }

        void StopJob(Job job)
        {
            
        }
    }
}
