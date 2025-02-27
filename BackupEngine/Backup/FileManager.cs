using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Settings;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    public class FileManager
    {
        /// <summary>
        /// Manages the logging of file transfer operations.
        /// </summary>        /// </summary>
        private SaveStrategy _saveStrategy;
        /// <summary>
        /// Manages the logging of file transfer operations.
        /// </summary>
        private readonly FileTransferLogManager _logManager;
        /// <summary>
        /// Handles the retrieval and management of application settings.
        /// </summary>
        private readonly SettingsRepository _settingsRepository;
        /// <summary>
        /// Manages the state of ongoing backup operations.
        /// </summary>
        private readonly StateManager _stateManager;
        /// <summary>
        /// Updates the current save strategy with a new one.
        /// </summary>
        /// <param name="newStrategy">The new save strategy to be applied.</param>
        public FileManager(SaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
            _settingsRepository = new SettingsRepository();
            _logManager = new FileTransferLogManager(_settingsRepository.GetLogPath().GetAbsolutePath(), _settingsRepository.GetLogType());
            _stateManager = new StateManager();
        }
        /// <summary>
        /// Updates the current save strategy with a new one.
        /// </summary>
        /// <param name="newStrategy">The new save strategy to be applied.</param>
        public void SetSaveStrategy(SaveStrategy newStrategy)
        {
            _saveStrategy = newStrategy;
        }
        /// <summary>
        /// Executes the backup process based on the provided configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration containing source and destination paths.</param>
        /// <param name="waitHandle">An event handle used for managing pause and resume functionality.</param>
        public void Save(BackupConfiguration configuration, EventWaitHandle waitHandle)
        {
            string destinationBasePath = configuration.DestinationPath.GetAbsolutePath();

            if (!Directory.Exists(destinationBasePath))
            {
                Directory.CreateDirectory(destinationBasePath);
            }

            // Générer un nom unique pour le dossier de sauvegarde
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string uniqueDestinationPath = Path.Combine(destinationBasePath, $"{timestamp}_{configuration.Name}");

            // Créer le dossier de sauvegarde
            Directory.CreateDirectory(uniqueDestinationPath);

            _saveStrategy.Transfer += _logManager.OnTransfer;

            // Ajouter un écouteur pour l'événement StateUpdated
            _saveStrategy.StateUpdated += _stateManager.OnStateUpdated;

            // Lancer la sauvegarde avec le bon dossier
            _saveStrategy.Save(uniqueDestinationPath, waitHandle);
        }
        /// <summary>
        /// Subscribes a handler to track the progress of the backup process.
        /// </summary>
        /// <param name="handler">The event handler that receives progress updates.</param>
        public void SubscribeProgress(EventHandler<ProgressEvent> handler)
        {
            _saveStrategy.Progress += handler;
        }
    }
}