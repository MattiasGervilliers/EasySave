using BackupEngine.Shared;
using Newtonsoft.Json;
using LogLib;

namespace BackupEngine.Settings
{
    public class Settings : IJsonSerializable
    {
        public Language Language { get; set; }
        public CustomPath LogPath { get; set; }
        public CustomPath StatePath { get; set; }
        public List<BackupConfiguration> Configurations { get; set; }
        public LogType LogFormat { get; set; }
        private string _encryptionKey { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new CustomPath("logs");
            LogFormat = LogType.Json;
            StatePath = new CustomPath("logs/state.json");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            _encryptionKey=  new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
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
                LogFormat = jsonSettings.LogFormat;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
