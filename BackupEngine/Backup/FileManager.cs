using BackupEngine.Log;
using BackupEngine.State;
using BackupEngine.Settings;
using System;
using System.IO;

namespace BackupEngine.Backup
{
    /// <summary>
    /// The FileManager class is responsible for managing the backup.
    /// It configures and uses a backup strategy, while logging events and updating the state.
    /// </summary>
    public class FileManager
    {
        private SaveStrategy _saveStrategy;
        private readonly FileTransferLogManager _logManager;
        private readonly SettingsRepository _settingsRepository;
        private readonly StateManager _stateManager;

        /// <summary>
        /// Constructor of the FileManager class. It takes a backup strategy as a parameter and initializes the other components.
        /// </summary>
        public FileManager(SaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
            _settingsRepository = new SettingsRepository();
            _logManager = new FileTransferLogManager(_settingsRepository.GetLogPath().GetAbsolutePath(), _settingsRepository.GetLogType());
            _stateManager = new StateManager();
        }

        /// <summary>
        /// Method to change the backup strategy used by the FileManager.
        /// </summary>
        public void SetSaveStrategy(SaveStrategy newStrategy)
        {
            _saveStrategy = newStrategy;
        }

        /// <summary>
        /// Method that starts the backup by creating a unique destination folder and using the defined backup strategy.
        /// </summary>
        public void Save(BackupConfiguration configuration)
        {
            string destinationBasePath = configuration.DestinationPath.GetAbsolutePath();

            if (!Directory.Exists(destinationBasePath))
            {
                Directory.CreateDirectory(destinationBasePath);
            }

            /// <summary>
            /// Generate a unique name for the backup folder using a timestamp.
            /// </summary>
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string uniqueDestinationPath = Path.Combine(destinationBasePath, $"{timestamp}_{configuration.Name}");

            /// <summary>
            /// Create the backup folder.
            /// </summary>
            Directory.CreateDirectory(uniqueDestinationPath);

            _saveStrategy.Transfer += _logManager.OnTransfer;

            /// <summary>
            /// Add a listener for the StateUpdated event.
            /// </summary>
            _saveStrategy.StateUpdated += _stateManager.OnStateUpdated;

            /// <summary>
            /// Start the backup with the correct folder.
            /// </summary>
            _saveStrategy.Save(uniqueDestinationPath);
        }
    }
}
