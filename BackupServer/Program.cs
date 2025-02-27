using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;
using BackupServer;
using System.ComponentModel.DataAnnotations;

namespace BackupSrv
{
    class Program
    {
        /// <summary>
        /// The server instance responsible for handling remote commands.
        /// </summary>
        private static Server _server = new Server(5000, HandleRemoteCommand);

        /// <summary>
        /// Manages the execution and control of backup jobs.
        /// </summary>
        private static readonly JobManager _jobManager = new JobManager();

        /// <summary>
        /// Repository for managing application settings and configurations.
        /// </summary>
        private static SettingsRepository _settingsRepository = new SettingsRepository();

        /// <summary>
        /// Dictionary storing active backup jobs associated with their configurations.
        /// </summary>
        private static readonly Dictionary<BackupConfiguration, Job> _jobs = new Dictionary<BackupConfiguration, Job>();

        /// <summary>
        /// Event triggered to update backup progress.
        /// </summary>
        public event Action<BackupConfiguration, double>? ProgressUpdated;

        /// <summary>
        /// Starts the server to listen for remote commands.
        /// </summary>
        public static void StartServer()
        {
            _server.Start();
        }

        /// <summary>
        /// Handles commands received from the remote console.
        /// </summary>
        /// <param name="command">The command received from the remote console.</param>
        private static void HandleRemoteCommand(string command)
        {
            if (command == "pause")
            {
                Console.WriteLine("[Remote] Pause de la sauvegarde demandée.");
                foreach (var config in _settingsRepository.GetConfigurations())
                {
                    StopBackup(config);
                }
            }
            else if (command == "resume")
            {
                Console.WriteLine("[Remote] Reprise de la sauvegarde demandée.");
                foreach (var config in _settingsRepository.GetConfigurations())
                {
                    ResumeBackup(config);
                }
            }
            else if (command == "stop")
            {
                Console.WriteLine("[Remote] Arrêt de la sauvegarde demandée.");
                foreach (BackupConfiguration config in _settingsRepository.GetConfigurations())
                {
                    StopBackup(config);
                }
            }
        }
        /// <summary>
        /// Initializes and launches backup jobs for all stored configurations.
        /// </summary>
        private static void InitializeJobs()
        {
            foreach (var config in _settingsRepository.GetConfigurations())
            {
                if (!_jobs.ContainsKey(config))
                {
                    // Lancement du backup via JobManager et ajout dans le dictionnaire
                    Job job = _jobManager.LaunchBackup(config);
                    _jobs.Add(config, job);
                }
            }
        }
        /// <summary>
        /// Stops an ongoing backup associated with a specific configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to stop.</param>
        public static void StopBackup(BackupConfiguration configuration)
        {
            if (_jobs.TryGetValue(configuration, out Job job))
            {
                // Utilisation de CancelJob qui propage l'annulation via le CancellationToken
                _jobManager.CancelJob(job);
                _jobs.Remove(configuration);
                Console.WriteLine($"{configuration.Name} arrêté");
            }
            else
            {
                Console.WriteLine($"Aucun job trouvé pour la configuration {configuration.Name}");
            }
        }
        /// <summary>
        /// Pauses an ongoing backup associated with a specific configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to pause.</param>
        public static void PauseBackup(BackupConfiguration configuration)
        {
            if (_jobs.TryGetValue(configuration, out Job job))
            {
                _jobManager.PauseJob(job);
                Console.WriteLine($"{configuration.Name} en pause");
            }
        }

        /// <summary>
        /// Resumes a paused backup associated with a specific configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to resume.</param>
        public static void ResumeBackup(BackupConfiguration configuration)
        {
            if (_jobs.TryGetValue(configuration, out Job job))
            {
                _jobManager.ResumeJob(job);
                Console.WriteLine($"{configuration.Name} repris");
            }
        }

        static void Main(string[] args)
        {
            InitializeJobs();
            StartServer();
            Console.WriteLine("Le serveur écoute.");
            Console.ReadLine();
        }
    }
}
