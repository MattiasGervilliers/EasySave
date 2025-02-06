using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BackupEngine.SettingsRepository
{
    public class Settings : IJsonSerializable
    {
        public Language Language { get; set; }
        public Chemin LogPath { get; set; }
        public List<BackupConfiguration> Configurations { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new Chemin("logs");
        }

        public void FromJson(string json)
        {
            Settings jsonSettings = JsonConvert.DeserializeObject<Settings>(json);
            if (jsonSettings != null)
            {
                Language = jsonSettings.Language;
                LogPath = jsonSettings.LogPath;
                Configurations = jsonSettings.Configurations;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
