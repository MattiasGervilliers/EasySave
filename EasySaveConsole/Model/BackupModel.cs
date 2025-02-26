using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;

namespace EasySaveConsole.Model
{
    /// <summary>
    /// La classe BackupModel gère les opérations liées à la configuration et à l'exécution des sauvegardes.
    /// Elle interagit avec le JobManager pour lancer les sauvegardes et avec le SettingsRepository pour gérer
    /// les configurations de sauvegarde.
    /// </summary>
    public static class BackupModel
    {
        // Le gestionnaire de jobs pour gérer les sauvegardes
        private static readonly JobManager JobManager;

        // Le dépôt de paramètres pour accéder et manipuler les configurations de sauvegarde
        private static readonly SettingsRepository SettingsRepository;

        /// <summary>
        /// Constructeur statique de la classe BackupModel. Il initialise les instances de JobManager et de SettingsRepository.
        /// </summary>
        static BackupModel()
        {
            JobManager = new JobManager();  // Initialisation du gestionnaire de jobs
            SettingsRepository = new SettingsRepository();  // Initialisation du dépôt des paramètres
        }

        /// <summary>
        /// Ajoute une nouvelle configuration de sauvegarde au dépôt des paramètres.
        /// </summary>
        /// <param name="backupConfiguration">La configuration de sauvegarde à ajouter.</param>
        public static void AddConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.AddConfiguration(backupConfiguration);  // Ajout de la configuration
        }

        /// <summary>
        /// Supprime une configuration de sauvegarde du dépôt des paramètres.
        /// </summary>
        /// <param name="backupConfiguration">La configuration de sauvegarde à supprimer.</param>
        public static void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.DeleteConfiguration(backupConfiguration);  // Suppression de la configuration
        }

        /// <summary>
        /// Lance une sauvegarde pour une configuration donnée.
        /// </summary>
        /// <param name="backupConfiguration">La configuration de sauvegarde à exécuter.</param>
        public static void LaunchConfig(BackupConfiguration backupConfiguration)
        {
            JobManager.LaunchBackup(backupConfiguration);  // Lancement de la sauvegarde
        }

        /// <summary>
        /// Lance une sauvegarde pour plusieurs configurations.
        /// </summary>
        /// <param name="backupConfigurations">La liste des configurations de sauvegarde à exécuter.</param>
        public static void LaunchConfigs(List<BackupConfiguration> backupConfigurations)
        {
            JobManager.LaunchBackup(backupConfigurations);  // Lancement de la sauvegarde pour plusieurs configurations
        }

        /// <summary>
        /// Récupère la liste des configurations de sauvegarde enregistrées dans le dépôt des paramètres.
        /// </summary>
        /// <returns>La liste des configurations de sauvegarde.</returns>
        public static List<BackupConfiguration> GetConfigs()
        {
            return SettingsRepository.GetConfigurations();  // Retourne les configurations
        }

        /// <summary>
        /// Recherche une configuration de sauvegarde par son nom.
        /// </summary>
        /// <param name="name">Le nom de la configuration de sauvegarde à trouver.</param>
        /// <returns>La configuration de sauvegarde correspondant au nom spécifié, ou null si non trouvée.</returns>
        public static BackupConfiguration? FindConfig(string name)
        {
            return SettingsRepository.GetConfiguration(name);  // Recherche la configuration par nom
        }

        /// <summary>
        /// Recherche une configuration de sauvegarde par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la configuration de sauvegarde à trouver.</param>
        /// <returns>La configuration de sauvegarde correspondant à l'identifiant, ou null si non trouvée.</returns>
        public static BackupConfiguration? FindConfig(int id)
        {
            return SettingsRepository.GetConfigurationById(id);  // Recherche la configuration par identifiant
        }

        /// <summary>
        /// Récupère la langue actuellement définie dans les paramètres.
        /// </summary>
        /// <returns>La langue actuellement définie.</returns>
        public static Language? GetLanguage()
        {
            return SettingsRepository.GetLanguage();  // Retourne la langue définie dans les paramètres
        }

        /// <summary>
        /// Met à jour la langue des paramètres.
        /// </summary>
        /// <param name="language">La nouvelle langue à définir.</param>
        public static void UpdateLanguage(Language language)
        {
            SettingsRepository.UpdateLanguage(language);  // Mise à jour de la langue dans les paramètres
        }
    }
}
