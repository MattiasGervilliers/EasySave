using BackupEngine.Log;
using BackupEngine.State;
using BackupEngine.Settings;
using System;
using System.IO;

namespace BackupEngine.Backup
{
    /// <summary>
    /// La classe FileManager est responsable de la gestion de la sauvegarde.
    /// Elle configure et utilise une stratégie de sauvegarde, tout en loguant les événements et en mettant à jour l'état.
    /// </summary>
    public class FileManager
    {
        private SaveStrategy _saveStrategy;
        private readonly FileTransferLogManager _logManager;
        private readonly SettingsRepository _settingsRepository;
        private readonly StateManager _stateManager;

        /// <summary>
        /// Constructeur de la classe FileManager. Il prend en paramètre une stratégie de sauvegarde et initialise les autres composants.
        /// </summary>
        public FileManager(SaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
            _settingsRepository = new SettingsRepository();
            _logManager = new FileTransferLogManager(_settingsRepository.GetLogPath().GetAbsolutePath(), _settingsRepository.GetLogType());
            _stateManager = new StateManager();
        }

        /// <summary>
        /// Méthode pour changer la stratégie de sauvegarde utilisée par le FileManager.
        /// </summary>
        public void SetSaveStrategy(SaveStrategy newStrategy)
        {
            _saveStrategy = newStrategy;
        }

        /// <summary>
        /// Méthode qui lance la sauvegarde en créant un dossier de destination unique et en utilisant la stratégie de sauvegarde définie.
        /// </summary>
        public void Save(BackupConfiguration configuration)
        {
            string destinationBasePath = configuration.DestinationPath.GetAbsolutePath();

            if (!Directory.Exists(destinationBasePath))
            {
                Directory.CreateDirectory(destinationBasePath);
            }

            /// <summary>
            /// Générer un nom unique pour le dossier de sauvegarde en utilisant un timestamp.
            /// </summary>
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string uniqueDestinationPath = Path.Combine(destinationBasePath, $"{timestamp}_{configuration.Name}");

            /// <summary>
            /// Créer le dossier de sauvegarde.
            /// </summary>
            Directory.CreateDirectory(uniqueDestinationPath);

            _saveStrategy.Transfer += _logManager.OnTransfer;

            /// <summary>
            /// Ajouter un écouteur pour l'événement StateUpdated.
            /// </summary>
            _saveStrategy.StateUpdated += _stateManager.OnStateUpdated;

            /// <summary>
            /// Lancer la sauvegarde avec le bon dossier.
            /// </summary>
            _saveStrategy.Save(uniqueDestinationPath);
        }
    }
}
