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
        public HashSet<string> ExtensionPriority { get; set; }
        public List<string> BusinessSoftwareList { get; set; }

        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new CustomPath("logs");
            LogFormat = LogType.Json;
            StatePath = new CustomPath("logs");
            ExtensionPriority = new HashSet<string>() { ".txt", ".pdf" };// Pour l'instant la priorité des extensions est set ici
            BusinessSoftwareList = new List<string> { "CalculatorApp", "msedge" };//idem pour les logitiels metiers
            Theme = Theme.Light;
            Random random = new Random();   
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
