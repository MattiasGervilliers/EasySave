using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;
using BackupServer;
using System.ComponentModel.DataAnnotations;

namespace BackupSrv
{
    class Program
    {
        private static Server _server = new Server(5000, HandleRemoteCommand);
        private static readonly JobManager _jobManager = new JobManager();
        private static SettingsRepository _settingsRepository = new SettingsRepository();
        // Initialisation correcte du dictionnaire
        private static readonly Dictionary<BackupConfiguration, Job> _jobs = new Dictionary<BackupConfiguration, Job>();

        public event Action<BackupConfiguration, double>? ProgressUpdated;

        public static void StartServer()
        {
            _server.Start();
        }

        // Gestion des commandes reçues depuis la console déportée
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

        // Initialisation et lancement des jobs pour chaque configuration
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

        public static void PauseBackup(BackupConfiguration configuration)
        {
            if (_jobs.TryGetValue(configuration, out Job job))
            {
                _jobManager.PauseJob(job);
                Console.WriteLine($"{configuration.Name} en pause");
            }
        }

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
