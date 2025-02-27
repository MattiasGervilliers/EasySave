using BackupEngine.Shared;
using LogLib;
using Newtonsoft.Json;

namespace BackupEngine.Settings
{
    public class SettingsRepository
    {
        /// <summary>
        /// The file path where the settings are stored in JSON format.
        /// </summary>
        private static readonly string _settingsPath = "settings.json";
        /// <summary>
        /// The current settings instance containing configurations and preferences.
        /// </summary>
        private Settings Settings { get; set; }
        /// <summary>
        /// Initializes a new instance of the SettingsRepository class and loads existing settings.
        /// </summary>
        public SettingsRepository() {
            Settings = Load();
        }
        /// <summary>
        /// Loads settings from the settings file if it exists, otherwise initializes new settings.
        /// </summary>
        /// <returns>A Settings instance containing the application configurations.</returns>
        Settings Load()
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                Settings settings = new Settings();
                settings.FromJson(json);
                return settings;
            }
            else
            {
                CreateFile();
                return new Settings();
            }
        }
        /// <summary>
        /// Creates a new settings file if it does not exist.
        /// </summary>
        private void CreateFile()
        {
            FileStream fs = File.Create(_settingsPath);
            fs.Close();
        }
        /// <summary>
        /// Saves the current settings to the settings file in JSON format.
        /// </summary>
        private void SaveSettings()
        {
            string json = Settings.ToJson();
            File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
        }
        /// <summary>
        /// Adds a new backup configuration to the settings.
        /// </summary>
        /// <param name="configuration">The backup configuration to add.</param>
        public void AddConfiguration(BackupConfiguration configuration)
        {
            if (Settings.Configurations.Count >= 5)
            {
                throw new System.Exception("You can't have more than 5 configurations.");
            }
            Settings.Configurations.Add(configuration);
            SaveSettings();
        }
        /// <summary>
        /// Deletes an existing backup configuration from the settings.
        /// </summary>
        /// <param name="configuration">The backup configuration to remove.</param>
        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            Settings.Configurations.Remove(configuration);
            SaveSettings();
        }
        /// <summary>
        /// Updates an existing backup configuration in the settings.
        /// </summary>
        /// <param name="configuration">The backup configuration with updated values.</param>
        public void UpdateConfiguration(BackupConfiguration configuration)
        {
            int index = Settings.Configurations.FindIndex(c => c.Name == configuration.Name);
            if (index == -1)
            {
                throw new System.Exception("Configuration not found.");
            }
            Settings.Configurations[index] = configuration;
            SaveSettings();
        }
        /// <summary>
        /// Retrieves the list of all backup configurations.
        /// </summary>
        /// <returns>A list of backup configurations.</returns>
        public List<BackupConfiguration> GetConfigurations()
        {
            return Settings.Configurations;
        }
        /// <summary>
        /// Retrieves the list of business software that may affect backup operations.
        /// </summary>
        /// <returns>A list of business software names.</returns>
        public List<string> GetBusinessSoftwareList()
        {
            return Settings.BusinessSoftwareList;
        }
        /// <summary>
        /// Updates the application language preference.
        /// </summary>
        /// <param name="language">The new language to set.</param>
        public void UpdateLanguage(Language language)
        {
            Settings.Language = language;
            SaveSettings();
        }
        /// <summary>
        /// Updates the list of file extensions that should be prioritized during backup.
        /// </summary>
        /// <param name="extensionPriority">A set of prioritized file extensions.</param>
        public void UpdateExtensionPriority(HashSet<string> extensionPriority)
        {
            Settings.ExtensionPriority = extensionPriority;
            SaveSettings();
        }
        /// <summary>
        /// Retrieves the list of prioritized file extensions for backup.
        /// </summary>
        /// <returns>A set of file extensions.</returns>
        public HashSet<string> GetExtensionPriority()
        {
            return Settings.ExtensionPriority;
        }
        /// <summary>
        /// Updates the log file storage path.
        /// </summary>
        /// <param name="logPath">The new log directory path.</param>
        public void UpdateLogPath(CustomPath logPath)
        {
            Settings.LogPath = logPath;
            SaveSettings();
        }
        /// <summary>
        /// Retrieves the current log file storage path.
        /// </summary>
        /// <returns>The log directory path.</returns>
        public CustomPath GetLogPath()
        {
            return Settings.LogPath;
        }
        /// <summary>
        /// Retrieves the current language preference.
        /// </summary>
        /// <returns>The currently set language.</returns>
        public Language GetLanguage()
        {
            return Settings.Language;
        }
        /// <summary>
        /// Retrieves the current theme preference.
        /// </summary>
        /// <returns>The currently set theme.</returns>
        public Theme GetTheme()
        {
            return Settings.Theme;
        }
        /// <summary>
        /// Updates the application theme.
        /// </summary>
        /// <param name="theme">The new theme to apply.</param>
        public void UpdateTheme(Theme theme)
        {
            Settings.Theme = theme;
            SaveSettings();
        }
        /// <summary>
        /// Retrieves a backup configuration by its name.
        /// </summary>
        /// <param name="name">The name of the backup configuration.</param>
        /// <returns>The backup configuration if found, otherwise null.</returns>
        public BackupConfiguration? GetConfiguration(string name)
        {
            return Settings.Configurations.Find(c => c.Name == name);
        }
        /// <summary>
        /// Retrieves a backup configuration by its index ID.
        /// </summary>
        /// <param name="id">The index ID of the configuration.</param>
        /// <returns>The backup configuration if found.</returns>
        public BackupConfiguration? GetConfigurationById(int id)
        {
            return Settings.Configurations[id];
        }
        /// <summary>
        /// Retrieves the current state file storage path.
        /// </summary>
        /// <returns>The state file directory path.</returns>
        public CustomPath GetStatePath()
        {
            return Settings.StatePath;
        }
        /// <summary>
        /// Updates the state file storage path.
        /// </summary>
        /// <param name="statePath">The new state file directory path.</param>
        public void UpdateStatePath(string statePath)
        {
            Settings.StatePath = new CustomPath(statePath);
            SaveSettings();
        }
        /// <summary>
        /// Retrieves the log format type used for logging backup operations.
        /// </summary>
        /// <returns>The current log format type.</returns>
        public LogType GetLogType()
        {
            return Settings.LogFormat;
        }
        /// <summary>
        /// Updates the log format type.
        /// </summary>
        /// <param name="logType">The new log format type to apply.</param>
        public void UpdateLogType(LogType logType)
        {
            Settings.LogFormat = logType;
            SaveSettings();
        }
    }
}
