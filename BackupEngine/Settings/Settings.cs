using BackupEngine.Shared;
using Newtonsoft.Json;
using LogLib;

namespace BackupEngine.Settings
{
    /// <summary>
    /// The Settings class represents the configuration settings for the application.
    /// It includes the language, log and state paths, as well as a list of backup configurations.
    /// It also implements the IJsonSerializable interface to facilitate serialization and deserialization in JSON format.
    /// </summary>
    public class Settings : IJsonSerializable
    {
        /// <summary>
        /// The language chosen for the application (e.g., English, French).
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// The path to the folder where log files are stored.
        /// </summary>
        public CustomPath LogPath { get; set; }

        /// <summary>
        /// The path to the application state file (e.g., to store backup states).
        /// </summary>
        public CustomPath StatePath { get; set; }

        /// <summary>
        /// A list of backup configurations defining different types of backups and their parameters.
        /// </summary>
        public List<BackupConfiguration> Configurations { get; set; }
        public Theme Theme { get; set; }
        /// <summary>
        /// The log format used (e.g., JSON or text).
        /// </summary>
        public LogType LogFormat { get; set; }
        public HashSet<string> ExtensionPriority { get; set; }
        public List<string> BusinessSoftwareList { get; set; }

        /// <summary>
        /// Constructor for the Settings class that initializes the default settings.
        /// The language is set to English, the log and state paths are set by default,
        /// and the log format is JSON.
        /// </summary>
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

        /// <summary>
        /// Deserializes a JSON string to initialize the properties of the Settings object.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
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
        /// Serializes the Settings object into a JSON string.
        /// </summary>
        /// <returns>The JSON string representing the Settings object.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
