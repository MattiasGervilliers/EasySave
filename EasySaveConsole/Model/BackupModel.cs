using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;

namespace EasySaveConsole.Model
{
    /// <summary>
    /// The BackupModel class manages operations related to backup configuration and execution.
    /// It interacts with the JobManager to start backups and with the SettingsRepository to manage
    /// backup configurations.
    /// </summary>
    public static class BackupModel
    {
        // The job manager to manage backups
        private static readonly JobManager JobManager;

        // The settings repository to access and manipulate backup configurations
        private static readonly SettingsRepository SettingsRepository;

        /// <summary>
        /// Static constructor for the BackupModel class. It initializes the instances of JobManager and SettingsRepository.
        /// </summary>
        static BackupModel()
        {
            JobManager = new JobManager();  // Initializes the job manager
            SettingsRepository = new SettingsRepository();  // Initializes the settings repository
        }

        /// <summary>
        /// Adds a new backup configuration to the settings repository.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to add.</param>
        public static void AddConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.AddConfiguration(backupConfiguration);  // Adds the configuration
        }

        /// <summary>
        /// Deletes a backup configuration from the settings repository.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to delete.</param>
        public static void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.DeleteConfiguration(backupConfiguration);  // Deletes the configuration
        }

        /// <summary>
        /// Starts a backup for a given configuration.
        /// </summary>
        /// <param name="backupConfiguration">The backup configuration to execute.</param>
        public static void LaunchConfig(BackupConfiguration backupConfiguration)
        {
            JobManager.LaunchBackup(backupConfiguration);  // Starts the backup
        }

        /// <summary>
        /// Starts backups for multiple configurations.
        /// </summary>
        /// <param name="backupConfigurations">The list of backup configurations to execute.</param>
        public static void LaunchConfigs(List<BackupConfiguration> backupConfigurations)
        {
            JobManager.LaunchBackup(backupConfigurations);  // Starts backups for multiple configurations
        }

        /// <summary>
        /// Retrieves the list of backup configurations stored in the settings repository.
        /// </summary>
        /// <returns>The list of backup configurations.</returns>
        public static List<BackupConfiguration> GetConfigs()
        {
            return SettingsRepository.GetConfigurations();  // Returns the configurations
        }

        /// <summary>
        /// Searches for a backup configuration by its name.
        /// </summary>
        /// <param name="name">The name of the backup configuration to find.</param>
        /// <returns>The backup configuration corresponding to the specified name, or null if not found.</returns>
        public static BackupConfiguration? FindConfig(string name)
        {
            return SettingsRepository.GetConfiguration(name);  // Searches for the configuration by name
        }

        /// <summary>
        /// Searches for a backup configuration by its ID.
        /// </summary>
        /// <param name="id">The ID of the backup configuration to find.</param>
        /// <returns>The backup configuration corresponding to the ID, or null if not found.</returns>
        public static BackupConfiguration? FindConfig(int id)
        {
            return SettingsRepository.GetConfigurationById(id);  // Searches for the configuration by ID
        }

        /// <summary>
        /// Retrieves the currently defined language in the settings.
        /// </summary>
        /// <returns>The currently defined language.</returns>
        public static Language? GetLanguage()
        {
            return SettingsRepository.GetLanguage();  // Returns the language set in the settings
        }

        /// <summary>
        /// Updates the language in the settings.
        /// </summary>
        /// <param name="language">The new language to set.</param>
        public static void UpdateLanguage(Language language)
        {
            SettingsRepository.UpdateLanguage(language);  // Updates the language in the settings
        }
    }
}
