using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine.Settings
{
    public class Settings : IJsonSerializable
    {
        public Language Language { get; set; }
        public Chemin LogPath { get; set; }
        public string StatePath { get; set; }
        public List<BackupConfiguration> Configurations { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new Chemin("logs");
            StatePath = "logs/state.json";
        }

        public void FromJson(string json)
        {
            Settings jsonSettings = JsonConvert.DeserializeObject<Settings>(json);
            if (jsonSettings != null)
            {
                Language = jsonSettings.Language;
                LogPath = jsonSettings.LogPath;
                StatePath = jsonSettings.StatePath;
                Configurations = jsonSettings.Configurations;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
