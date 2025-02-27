using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;
using static System.Reflection.Metadata.BlobBuilder;

namespace EasySaveConsole.Model
{
    public static class BackupModel
    {
        /// <summary>
        /// Manages backup jobs and configurations by interacting with JobManager and SettingsRepository.
        /// </summary>
        private static readonly JobManager _jobManager;

        /// <summary>
        /// Manages application settings, including backup configurations.
        /// </summary>
        private static readonly SettingsRepository _settingsRepository;

        /// <summary>
        /// Dictionary storing active backup jobs associated with their configurations.
        /// </summary>
        private static readonly Dictionary<BackupConfiguration, Job> _jobs = [];

        /// <summary>
        /// Event triggered to update the progress of backup operations.
        /// </summary>
        public static event Action<BackupConfiguration, double>? ProgressUpdated;

        /// <summary>
        /// Static constructor initializing JobManager and SettingsRepository.
        /// </summary>
        static BackupModel()
        {
            _jobManager = new JobManager();
            _settingsRepository = new SettingsRepository();
        }
        /// <summary>
        /// Adds a new backup configuration to the repository.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to be added.</param>
        public static void AddConfig(BackupConfiguration backupConfiguration)
        {
            _settingsRepository.AddConfiguration(backupConfiguration);
        }
        /// <summary>
        /// Deletes a backup configuration from the repository.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to be removed.</param>
        public static void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            _settingsRepository.DeleteConfiguration(backupConfiguration);
        }
        /// <summary>
        /// Launches a backup operation for the specified configuration.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to execute.</param>
        public static void LaunchConfig(BackupConfiguration backupConfiguration)
        {
            _jobManager.LaunchBackup(backupConfiguration);
        }
        /// <summary>
        /// Launches backup operations for multiple configurations.
        /// </summary>
        /// <param name="configurations">The list of backup configurations to execute.</param>

        public static void LaunchConfigs(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = _jobManager.LaunchBackup(configurations);

            for (int i = 0; i < configurations.Count; ++i)
            {
                _jobs.Add(configurations[i], jobs[i]);
                BackupConfiguration conf = configurations[i];
                //jobs[i].ProgressChanged += (sender, progress) => OnProgressChange(conf, progress);--------------------?????????????
            }
        }
        /// <summary>
        /// Retrieves all stored backup configurations.
        /// </summary>
        /// <returns>A list of backup configurations.</returns>
        public static List<BackupConfiguration> GetConfigs()
        {
            return _settingsRepository.GetConfigurations();
        }
        /// <summary>
        /// Finds a backup configuration by its name.
        /// </summary>
        /// <param name="name">The name of the backup configuration.</param>
        /// <returns>The backup configuration if found, otherwise null.</returns>
        public static BackupConfiguration? FindConfig(string name)
        {
            return _settingsRepository.GetConfiguration(name);
        }
        /// <summary>
        /// Finds a backup configuration by its index ID.
        /// </summary>
        /// <param name="id">The index ID of the backup configuration.</param>
        /// <returns>The backup configuration if found, otherwise null.</returns>
        public static BackupConfiguration? FindConfig(int id)
        {
            return _settingsRepository.GetConfigurationById(id);
        }
        /// <summary>
        /// Retrieves the current language setting of the application.
        /// </summary>
        /// <returns>The current language setting.</returns>
        public static Language? GetLanguage()
        {
            return _settingsRepository.GetLanguage();
        }
        /// <summary>
        /// Updates the language setting of the application.
        /// </summary>
        /// <param name="language">The new language to apply.</param>
        public static void UpdateLanguage(Language language)
        {
            _settingsRepository.UpdateLanguage(language);
        }
    }
}
