using BackupEngine.Shared;
using Newtonsoft.Json;
using LogLib;

namespace BackupEngine.Settings
{
    /// <summary>
    /// La classe Settings représente les paramètres de configuration pour l'application.
    /// Elle inclut la langue, les chemins de log et d'état, ainsi qu'une liste de configurations de sauvegarde.
    /// Elle implémente également l'interface IJsonSerializable pour faciliter la sérialisation et la désérialisation au format JSON.
    /// </summary>
    public class Settings : IJsonSerializable
    {
        /// <summary>
        /// La langue choisie pour l'application (par exemple, English, Français).
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Le chemin du dossier où les fichiers de log sont stockés.
        /// </summary>
        public CustomPath LogPath { get; set; }

        /// <summary>
        /// Le chemin du fichier d'état de l'application (par exemple, pour stocker l'état des sauvegardes).
        /// </summary>
        public CustomPath StatePath { get; set; }

        /// <summary>
        /// Une liste de configurations de sauvegarde définissant les différents types de sauvegarde et leurs paramètres.
        /// </summary>
        public List<BackupConfiguration> Configurations { get; set; }

        /// <summary>
        /// Le format de log utilisé (par exemple, JSON ou texte).
        /// </summary>
        public LogType LogFormat { get; set; }

        /// <summary>
        /// Constructeur de la classe Settings qui initialise les paramètres par défaut.
        /// La langue est définie sur l'anglais, les chemins de logs et d'état sont définis par défaut,
        /// et le format de log est JSON.
        /// </summary>
        public Settings()
        {
            Configurations = new List<BackupConfiguration>();
            Language = Language.English;
            LogPath = new CustomPath("logs");
            LogFormat = LogType.Json;
            StatePath = new CustomPath("logs/state.json");
        }

        /// <summary>
        /// Désérialise une chaîne JSON pour initialiser les propriétés de l'objet Settings.
        /// </summary>
        /// <param name="json">La chaîne JSON à désérialiser.</param>
        public void FromJson(string json)
        {
            // Désérialise le JSON dans un objet Settings
            Settings jsonSettings = JsonConvert.DeserializeObject<Settings>(json);
            if (jsonSettings != null)
            {
                // Copie les propriétés de l'objet désérialisé dans l'objet actuel
                Language = jsonSettings.Language;
                LogPath = jsonSettings.LogPath;
                StatePath = jsonSettings.StatePath;
                Configurations = jsonSettings.Configurations;
                LogFormat = jsonSettings.LogFormat;
            }
        }

        /// <summary>
        /// Sérialise l'objet Settings en une chaîne JSON.
        /// </summary>
        /// <returns>La chaîne JSON représentant l'objet Settings.</returns>
        public string ToJson()
        {
            // Sérialise l'objet actuel en JSON avec une mise en forme indentée pour la lisibilité
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
