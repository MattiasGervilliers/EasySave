using BackupEngine.Shared;
using LogLib;
using Newtonsoft.Json;

namespace BackupEngine.Settings
{
    /// <summary>
    /// The SettingsRepository class manages the saving, loading, and modification of application configuration settings.
    /// It allows manipulation of the JSON configuration file and management of various backup configurations.
    /// </summary>
    public class SettingsRepository
    {
        /// <summary>
        /// The path of the configuration settings file.
        /// </summary>
        private static readonly string _settingsPath = "settings.json";

        /// <summary>
        /// The Settings object that contains all the configuration information.
        /// </summary>
        private Settings Settings { get; set; }

        /// <summary>
        /// Constructor for the SettingsRepository class.
        /// It loads the configuration settings from the settings.json file.
        /// </summary>
        public SettingsRepository()
        {
            Settings = Load();
        }

        /// <summary>
        /// Loads the settings from the JSON file.
        /// If the file exists, it deserializes the JSON content into a Settings object.
        /// Otherwise, it creates a new settings file.
        /// </summary>
        /// <returns>The Settings object containing data loaded from the JSON file.</returns>
        Settings Load()
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
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
        /// Creates an empty settings.json file if the file does not exist.
        /// </summary>
        private void CreateFile()
        {
            FileStream fs = File.Create(SettingsPath);
            fs.Close();
        }

        /// <summary>
        /// Saves the current settings in the settings.json file in indented JSON format.
        /// </summary>
        private void SaveSettings()
        {
            string json = Settings.ToJson();
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
        }

        /// <summary>
        /// Adds a new backup configuration to the list of configurations.
        /// If the number of configurations exceeds 5, an exception is thrown.
        /// </summary>
        /// <param name="configuration">The configuration to add.</param>
        public void AddConfiguration(BackupConfiguration configuration)
        {
            Settings.Configurations.Add(configuration);
            SaveSettings();
        }

        /// <summary>
        /// Deletes an existing backup configuration.
        /// </summary>
        /// <param name="configuration">The configuration to delete.</param>
        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            Settings.Configurations.Remove(configuration);
            SaveSettings();
        }

        /// <summary>
        /// Updates an existing backup configuration.
        /// If the configuration is not found, Create the configuration
        /// </summary>
        /// <param name="configuration">The new backup configuration.</param>
        public void UpdateOrCreateConfiguration(BackupConfiguration configuration, BackupConfiguration newBackup)
        {
            int index = Settings.Configurations.FindIndex(c => c.Name == configuration.Name);
            if (index == -1)
            {
                AddConfiguration(newBackup);
            }
            else
            {
                Settings.Configurations[index] = newBackup;
                SaveSettings();
            }
        }

        /// <summary>
        /// Returns all backup configurations.
        /// </summary>
        /// <returns>The list of backup configurations.</returns>
        public List<BackupConfiguration> GetConfigurations()
        {
            return Settings.Configurations;
        }
        public List<string> GetBusinessSoftwareList()
        {
            return Settings.BusinessSoftwareList;
        }

        /// <summary>
        /// Updates the application's language and saves the settings.
        /// </summary>
        /// <param name="language">The new language to apply.</param>
        public void UpdateLanguage(Language language)
        {
            Settings.Language = language;
            SaveSettings();
        }
        public void UpdateExtensionPriority(HashSet<string> extensionPriority)
        {
            Settings.ExtensionPriority = extensionPriority;
            SaveSettings();
        }
        public HashSet<string> GetExtensionPriority()
        {
            return Settings.ExtensionPriority;
        }

        /// <summary>
        /// Updates the log path and saves the settings.
        /// </summary>
        /// <param name="logPath">The new log path.</param>
        public void UpdateLogPath(CustomPath logPath)
        {
            Settings.LogPath = logPath;
            SaveSettings();
        }

        /// <summary>
        /// Returns the current log path.
        /// </summary>
        /// <returns>The log path.</returns>
        public CustomPath GetLogPath()
        {
            return Settings.LogPath;
        }

        /// <summary>
        /// Returns the language currently used in the settings.
        /// </summary>
        /// <returns>The current language.</returns>
        public Language GetLanguage()
        {
            return Settings.Language;
        }
        public Theme GetTheme()
        {
            return Settings.Theme;
        }
        public void UpdateTheme(Theme theme)
        {
            Settings.Theme = theme;
            SaveSettings();
        }

        /// <summary>
        /// Searches for a backup configuration by its name.
        /// </summary>
        /// <param name="name">The name of the configuration to search for.</param>
        /// <returns>The corresponding backup configuration, or null if not found.</returns>
        public BackupConfiguration? GetConfiguration(string name)
        {
            return Settings.Configurations.Find(c => c.Name == name);
        }

        /// <summary>
        /// Searches for a backup configuration by its ID in the list of configurations.
        /// </summary>
        /// <param name="id">The ID of the configuration to search for.</param>
        /// <returns>The corresponding backup configuration.</returns>
        public BackupConfiguration? GetConfigurationById(int id)
        {
            return Settings.Configurations[id];
        }

        /// <summary>
        /// Returns the current application state file path.
        /// </summary>
        /// <returns>The state file path.</returns>
        public CustomPath GetStatePath()
        {
            return Settings.StatePath;
        }

        /// <summary>
        /// Updates the state file path and saves the settings.
        /// </summary>
        /// <param name="statePath">The new state file path.</param>
        public void UpdateStatePath(string statePath)
        {
            Settings.StatePath = new CustomPath(statePath);
            SaveSettings();
        }

        /// <summary>
        /// Returns the current log format.
        /// </summary>
        /// <returns>The current log format type.</returns>
        public LogType GetLogType()
        {
            return Settings.LogFormat;
        }

        /// <summary>
        /// Updates the log format and saves the settings.
        /// </summary>
        /// <param name="logType">The new log format.</param>
        public void UpdateLogType(LogType logType)
        {
            Settings.LogFormat = logType;
            SaveSettings();
        }
    }
}
