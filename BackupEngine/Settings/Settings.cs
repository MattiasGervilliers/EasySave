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
        public Theme Theme { get; set; }
        public LogType LogFormat { get; set; }
        public string _encryptionKey { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new CustomPath("logs");
            LogFormat = LogType.Json;
            StatePath = new CustomPath("logs/state.json");
            Theme = Theme.Light;
            Random random = new Random();   
            _encryptionKey=  new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
                Theme = jsonSettings.Theme;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
