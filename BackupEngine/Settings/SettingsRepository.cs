using System.Collections.Generic;
using System.IO;

namespace BackupEngine.Settings
{
    internal class SettingsRepository
    {
        private static readonly string _settingsPath = "settings.json";
        private Settings Settings { get; set; }

        public SettingsRepository() {
            Settings = Load();
        }

        public Settings Load()
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
            File.Create(_settingsPath);
        }

        private void SaveSettings()
        {
            string json = Settings.ToJson();
            File.WriteAllText(_settingsPath, json);
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

        public void UpdateLogPath(Chemin logPath)
        {
            Settings.LogPath = logPath;
            SaveSettings();
        }
    }
}
