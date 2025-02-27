using BackupEngine.Shared;
using Newtonsoft.Json;
using LogLib;

namespace BackupEngine.Settings
{
    public class Settings : IJsonSerializable
    {
        /// <summary>
        /// The current language setting of the application.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// The directory path where log files are stored.
        /// </summary>
        public CustomPath LogPath { get; set; }

        /// <summary>
        /// The directory path where backup state files are stored.
        /// </summary>
        public CustomPath StatePath { get; set; }

        /// <summary>
        /// The list of backup configurations saved in the application.
        /// </summary>
        public List<BackupConfiguration> Configurations { get; set; }

        /// <summary>
        /// The current theme setting of the application.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// The format type used for logging backup operations.
        /// </summary>
        public LogType LogFormat { get; set; }

        /// <summary>
        /// The set of file extensions that are given priority during backup operations.
        /// </summary>
        public HashSet<string> ExtensionPriority { get; set; }

        /// <summary>
        /// The list of business-related software that might interfere with backup operations.
        /// </summary>
        public List<string> BusinessSoftwareList { get; set; }

        /// <summary>
        /// Initializes a new instance of the Settings class with default values.
        /// </summary>
        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new CustomPath("logs");
            LogFormat = LogType.Json;
            StatePath = new CustomPath("logs");
            ExtensionPriority = new HashSet<string>() { ".txt", ".pdf" };// Pour l'instant la priorité des extensions est set ici
            BusinessSoftwareList = new List<string> { "CalculatorApp" };//idem pour les logitiels metiers
            Theme = Theme.Light;
            Random random = new Random();   
        }
        /// <summary>
        /// Loads application settings from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string containing the settings data.</param>
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
        /// <summary>
        /// Serializes the current application settings into a JSON string.
        /// </summary>
        /// <returns>A formatted JSON string representing the settings.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
