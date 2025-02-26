using BackupEngine.Log;
using BackupEngine.Progress;
using BackupEngine.Settings;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    public class FileManager
    {
        private SaveStrategy _saveStrategy;
        private readonly FileTransferLogManager _logManager;
        private readonly SettingsRepository _settingsRepository;
        private readonly StateManager _stateManager;

        public FileManager(SaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
            _settingsRepository = new SettingsRepository();
            _logManager = new FileTransferLogManager(_settingsRepository.GetLogPath().GetAbsolutePath(), _settingsRepository.GetLogType());
            _stateManager = new StateManager();
        }

        public void SetSaveStrategy(SaveStrategy newStrategy)
        {
            _saveStrategy = newStrategy;
        }

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

        public void SubscribeProgress(EventHandler<ProgressEvent> handler)
        {
            _saveStrategy.Progress += handler;
        }
    }
}