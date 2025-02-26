using System.Collections.Generic;
using System.Threading;

namespace BackupEngine.Job
{
    /// <summary>
    /// Classe responsable de la gestion des jobs de sauvegarde.
    /// Elle permet de lancer des jobs de sauvegarde, chacun dans un thread séparé.
    /// </summary>
    public class JobManager
    {
        // Liste qui maintient les threads des jobs en cours d'exécution.
        private List<Thread> JobsThreads { get; set; }

        /// <summary>
        /// Constructeur de la classe JobManager.
        /// Initialise la liste des threads des jobs.
        /// </summary>
        public JobManager()
        {
            JobsThreads = new List<Thread>();  // Initialise la liste des threads des jobs
        }

        /// <summary>
        /// Lance un job de sauvegarde dans un thread séparé en utilisant la configuration donnée.
        /// </summary>
        /// <param name="configuration">La configuration de la sauvegarde à exécuter.</param>
        /// <returns>Le job de sauvegarde lancé.</returns>
        public Job LaunchBackup(BackupConfiguration configuration)
        {
            Job job = new Job(configuration);  // Crée un nouveau job avec la configuration fournie
            Thread thread = new Thread(() => job.Run());  // Crée un thread pour exécuter le job
            JobsThreads.Add(thread);  // Ajoute le thread à la liste des threads actifs
            thread.Start();  // Démarre le thread
            return job;  // Retourne le job lancé
        }

        /// <summary>
        /// Lance plusieurs jobs de sauvegarde dans des threads séparés, en utilisant une liste de configurations.
        /// </summary>
        /// <param name="configurations">La liste des configurations de sauvegarde à exécuter.</param>
        /// <returns>La liste des jobs de sauvegarde lancés.</returns>
        public List<Job> LaunchBackup(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = new List<Job>();  // Crée une nouvelle liste pour stocker les jobs lancés
            foreach (BackupConfiguration configuration in configurations)
            {
                jobs.Add(LaunchBackup(configuration));  // Lance chaque job et ajoute à la liste
            }
            return jobs;  // Retourne la liste des jobs lancés
        }

        /// <summary>
        /// Méthode privée pour arrêter un job de sauvegarde en cours d'exécution.
        /// Cette méthode est pour le moment non implémentée.
        /// </summary>
        /// <param name="job">Le job à arrêter.</param>
        void StopJob(Job job)
        {
            // Cette méthode pourrait contenir la logique pour arrêter un job en cours,
            // mais elle est actuellement vide et non implémentée.
        }
    }
}
