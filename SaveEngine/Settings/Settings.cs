using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BackupEngine.Settings
{
    internal class Settings : IJsonSerializable
    {
        public Language Language { get; set; }
        public Path LogPath { get; set; }
        public List<BackupConfiguration> Configurations { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new Path("logs");
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
