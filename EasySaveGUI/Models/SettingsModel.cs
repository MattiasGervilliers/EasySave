using BackupEngine;
using BackupEngine.Settings;
using BackupEngine.Shared;
using LogLib;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace EasySaveGUI.Models
{
    internal class SettingsModel
    {
        /// <summary>
        /// Repository managing application settings and backup configurations.
        /// </summary>
        private readonly SettingsRepository _settingsRepository = new SettingsRepository();
        /// <summary>
        /// Retrieves all stored backup configurations.
        /// </summary>
        /// <returns>A list of backup configurations.</returns>
        public List<BackupConfiguration> GetConfigurations()
        {
            return _settingsRepository.GetConfigurations();
        }
        /// <summary>
        /// Updates the application language setting.
        /// </summary>
        /// <param name="language">The new language to apply.</param>
        public void UpdateLanguage(string language)
        {
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), language));
        }
        /// <summary>
        /// Retrieves the current language setting of the application.
        /// </summary>
        /// <returns>The current language setting.</returns>
        public Language GetLanguage()
        {
            return _settingsRepository.GetLanguage();
        }
        /// <summary>
        /// Retrieves the directory path where log files are stored.
        /// </summary>
        /// <returns>The log directory path as a string.</returns>
        public string GetLogPath()
        {
            return _settingsRepository.GetLogPath().GetAbsolutePath();
        }
        /// <summary>
        /// Retrieves the directory path where backup state files are stored.
        /// </summary>
        /// <returns>The state file directory path as a string.</returns>
        public string GetStatePath()
        {
            return _settingsRepository.GetStatePath().GetAbsolutePath();
        }
        /// <summary>
        /// Retrieves the current log format type used for logging backup operations.
        /// </summary>
        /// <returns>The log format type as a string.</returns>
        public string GetLogType()
        {
            return _settingsRepository.GetLogType().ToString();
        }
        /// <summary>
        /// Updates the directory path where log files are stored.
        /// </summary>
        /// <param name="logPath">The new log directory path.</param>
        public void UpdateLogPath(string logPath)
        {
            _settingsRepository.UpdateLogPath(new CustomPath(logPath));
        }
        /// <summary>
        /// Updates the directory path where backup state files are stored.
        /// </summary>
        /// <param name="statePath">The new state file directory path.</param>
        public void UpdateStatePath(string statePath)
        {
            _settingsRepository.UpdateStatePath(statePath);
        }
        /// <summary>
        /// Updates the log format type used for logging backup operations.
        /// </summary>
        /// <param name="logType">The new log format type.</param>
        public void UpdateLogType(string logType)
        {
            _settingsRepository.UpdateLogType((LogType)Enum.Parse(typeof(LogType), logType));
        }
        /// <summary>
        /// Updates the application theme setting.
        /// </summary>
        /// <param name="theme">The new theme to apply.</param>
        public void UpdateTheme(string theme)
        {
            _settingsRepository.UpdateTheme((Theme)Enum.Parse(typeof(Theme), theme));
        }
        /// <summary>
        /// Retrieves the current theme setting of the application.
        /// </summary>
        /// <returns>The current theme setting.</returns>
        public Theme GetTheme()
        {
            return _settingsRepository.GetTheme();
        }
        // <summary>
        /// Creates a new backup configuration.
        /// </summary>
        /// <param name="Name">The name of the backup configuration.</param>
        /// <param name="SourcePath">The source directory path for the backup.</param>
        /// <param name="DestinationPath">The destination directory path for the backup.</param>
        /// <param name="backupType">The type of backup to be performed.</param>
        /// <param name="EncryptionKey">Optional encryption key for secure backups.</param>
        public void CreateConfiguration(
            string Name, 
            string SourcePath, 
            string DestinationPath, 
            BackupType backupType,
            string? EncryptionKey
        )
        {
            BackupConfiguration configuration = new BackupConfiguration
            {
                Name = Name,
                SourcePath = new CustomPath(SourcePath),
                DestinationPath = new CustomPath(DestinationPath),
                BackupType = backupType,
            };
        }
        /// <summary>
        /// Deletes a backup configuration from the repository.
        /// </summary>
        /// <param name="configuration">The backup configuration to remove.</param>
        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            _settingsRepository.DeleteConfiguration(configuration);
        }
    }
}
