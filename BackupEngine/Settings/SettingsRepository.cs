using BackupEngine.Shared;
using LogLib;
using Newtonsoft.Json;

namespace BackupEngine.Settings
{
    /// <summary>
    /// La classe SettingsRepository gère la sauvegarde, le chargement, et la modification des paramètres de configuration de l'application.
    /// Elle permet de manipuler le fichier de configuration JSON et de gérer les différentes configurations de sauvegarde.
    /// </summary>
    public class SettingsRepository
    {
        /// <summary>
        /// Le chemin du fichier de paramètres de configuration.
        /// </summary>
        private static readonly string _settingsPath = "settings.json";

        /// <summary>
        /// L'objet Settings qui contient toutes les informations de configuration.
        /// </summary>
        private Settings Settings { get; set; }

        /// <summary>
        /// Constructeur de la classe SettingsRepository.
        /// Il charge les paramètres de configuration à partir du fichier settings.json.
        /// </summary>
        public SettingsRepository()
        {
            Settings = Load();
        }

        /// <summary>
        /// Charge les paramètres depuis le fichier JSON.
        /// Si le fichier existe, il désérialise le contenu JSON en un objet Settings.
        /// Sinon, il crée un nouveau fichier de paramètres.
        /// </summary>
        /// <returns>L'objet Settings contenant les données chargées depuis le fichier JSON.</returns>
        Settings Load()
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

        /// <summary>
        /// Crée un fichier vide settings.json si le fichier n'existe pas.
        /// </summary>
        private void CreateFile()
        {
            FileStream fs = File.Create(_settingsPath);
            fs.Close();
        }

        /// <summary>
        /// Sauvegarde les paramètres actuels dans le fichier settings.json en format JSON indenté.
        /// </summary>
        private void SaveSettings()
        {
            string json = Settings.ToJson();
            File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
        }

        /// <summary>
        /// Ajoute une nouvelle configuration de sauvegarde à la liste des configurations.
        /// Si le nombre de configurations dépasse 5, une exception est lancée.
        /// </summary>
        /// <param name="configuration">La configuration à ajouter.</param>
        public void AddConfiguration(BackupConfiguration configuration)
        {
            if (Settings.Configurations.Count >= 5)
            {
                throw new System.Exception("You can't have more than 5 configurations.");
            }
            Settings.Configurations.Add(configuration);
            SaveSettings();
        }

        /// <summary>
        /// Supprime une configuration de sauvegarde existante.
        /// </summary>
        /// <param name="configuration">La configuration à supprimer.</param>
        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            Settings.Configurations.Remove(configuration);
            SaveSettings();
        }

        /// <summary>
        /// Met à jour une configuration de sauvegarde existante.
        /// Si la configuration n'est pas trouvée, une exception est lancée.
        /// </summary>
        /// <param name="configuration">La nouvelle configuration de sauvegarde.</param>
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

        /// <summary>
        /// Retourne toutes les configurations de sauvegarde.
        /// </summary>
        /// <returns>La liste des configurations de sauvegarde.</returns>
        public List<BackupConfiguration> GetConfigurations()
        {
            return Settings.Configurations;
        }

        /// <summary>
        /// Met à jour la langue de l'application et sauvegarde les paramètres.
        /// </summary>
        /// <param name="language">La nouvelle langue à appliquer.</param>
        public void UpdateLanguage(Language language)
        {
            Settings.Language = language;
            SaveSettings();
        }

        /// <summary>
        /// Met à jour le chemin des logs et sauvegarde les paramètres.
        /// </summary>
        /// <param name="logPath">Le nouveau chemin des logs.</param>
        public void UpdateLogPath(CustomPath logPath)
        {
            Settings.LogPath = logPath;
            SaveSettings();
        }

        /// <summary>
        /// Retourne le chemin des logs actuel.
        /// </summary>
        /// <returns>Le chemin des logs.</returns>
        public CustomPath GetLogPath()
        {
            return Settings.LogPath;
        }

        /// <summary>
        /// Retourne la langue actuellement utilisée dans les paramètres.
        /// </summary>
        /// <returns>La langue actuelle.</returns>
        public Language GetLanguage()
        {
            return Settings.Language;
        }

        /// <summary>
        /// Recherche une configuration de sauvegarde par son nom.
        /// </summary>
        /// <param name="name">Le nom de la configuration à rechercher.</param>
        /// <returns>La configuration de sauvegarde correspondante ou null si elle n'est pas trouvée.</returns>
        public BackupConfiguration? GetConfiguration(string name)
        {
            return Settings.Configurations.Find(c => c.Name == name);
        }

        /// <summary>
        /// Recherche une configuration de sauvegarde par son ID dans la liste des configurations.
        /// </summary>
        /// <param name="id">L'ID de la configuration à rechercher.</param>
        /// <returns>La configuration de sauvegarde correspondante.</returns>
        public BackupConfiguration? GetConfigurationById(int id)
        {
            return Settings.Configurations[id];
        }

        /// <summary>
        /// Retourne le chemin actuel du fichier d'état de l'application.
        /// </summary>
        /// <returns>Le chemin du fichier d'état.</returns>
        public CustomPath GetStatePath()
        {
            return Settings.StatePath;
        }

        /// <summary>
        /// Met à jour le chemin du fichier d'état et sauvegarde les paramètres.
        /// </summary>
        /// <param name="statePath">Le nouveau chemin pour le fichier d'état.</param>
        public void UpdateStatePath(string statePath)
        {
            Settings.StatePath = new CustomPath(statePath);
            SaveSettings();
        }

        /// <summary>
        /// Retourne le format actuel des logs.
        /// </summary>
        /// <returns>Le type de format de log actuel.</returns>
        public LogType GetLogType()
        {
            return Settings.LogFormat;
        }

        /// <summary>
        /// Met à jour le format des logs et sauvegarde les paramètres.
        /// </summary>
        /// <param name="logType">Le nouveau format des logs.</param>
        public void UpdateLogType(LogType logType)
        {
            Settings.LogFormat = logType;
            SaveSettings();
        }
    }
}
