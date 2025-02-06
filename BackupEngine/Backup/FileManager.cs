using BackupEngine.Log;
using BackupEngine.Settings;

namespace BackupEngine.Backup
{
    public class FileManager
    {
        private SaveStrategy _saveStrategy;
        private readonly FileTransferLogManager _logManager;
        private readonly SettingsRepository _settingsRepository;

        public FileManager(SaveStrategy saveStrategy)
        {
            this._saveStrategy = saveStrategy;
            _settingsRepository = new SettingsRepository();
            _logManager = new FileTransferLogManager(_settingsRepository.GetLogPath().GetAbsolutePath());
        }

        public void SetSaveStrategy(SaveStrategy newStrategy)
        {
            _saveStrategy = newStrategy;
        }

        public void Save(string sourcePath, string destinationBasePath)
        {
            if (!Directory.Exists(destinationBasePath))
            {
                Directory.CreateDirectory(destinationBasePath);
            }

            // Générer un nom unique pour le dossier de sauvegarde
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string uniqueDestinationPath = System.IO.Path.Combine(destinationBasePath, $"{timestamp}_Sauvegarde");

            // Créer le dossier de sauvegarde
            Directory.CreateDirectory(uniqueDestinationPath);

            _saveStrategy.Transfer += _logManager.OnTransfer;

            // Lancer la sauvegarde avec le bon dossier
            _saveStrategy.Save();
        }
    }
}