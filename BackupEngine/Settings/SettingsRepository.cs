using BackupEngine.Shared;
using LogLib;
using Newtonsoft.Json;

namespace BackupEngine.Settings
{
    public class SettingsRepository
    {
        private static readonly string _settingsPath = "settings.json";
        private Settings Settings { get; set; }

        public SettingsRepository() {
            Settings = Load();
        }

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

        private void CreateFile()
        {
            FileStream fs = File.Create(_settingsPath);
            fs.Close();
        }

        private void SaveSettings()
        {
            string json = Settings.ToJson();
            File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
        }

        public void AddConfiguration(BackupConfiguration configuration)
        {
            if (Settings.Configurations.Count >= 5)
            {
                throw new System.Exception("You can't have more than 5 configurations.");
            }
            Settings.Configurations.Add(configuration);
            SaveSettings();
        }

        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            Settings.Configurations.Remove(configuration);
            SaveSettings();
        }

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

        public List<BackupConfiguration> GetConfigurations()
        {
            return Settings.Configurations;
        }

        public void UpdateLanguage(Language language)
        {
            Settings.Language = language;
            SaveSettings();
        }

        public void UpdateLogPath(CustomPath logPath)
        {
            Settings.LogPath = logPath;
            SaveSettings();
        }

        public CustomPath GetLogPath()
        {
            return Settings.LogPath;
        }

        public Language GetLanguage()
        {
            return Settings.Language;
        }

        public BackupConfiguration? GetConfiguration(string name)
        {
            return Settings.Configurations.Find(c => c.Name == name);
        }

        public BackupConfiguration? GetConfigurationById(int id)
        {
            return Settings.Configurations[id];
        }

        public CustomPath GetStatePath()
        {
            return Settings.StatePath;
        }

        public void UpdateStatePath(string statePath)
        {
            Settings.StatePath = new CustomPath(statePath);
            SaveSettings();
        }

        public LogType GetLogType()
        {
            return Settings.LogFormat;
        }

        public void UpdateLogType(LogType logType)
        {
            Settings.LogFormat = logType;
            SaveSettings();
        }
    }
}
