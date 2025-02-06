using BackupEngine.Backup;
using BackupEngine.Log;
using BackupEngine.Settings;

public class FileManager
{
    private SaveStrategy saveStrategy;
    private FileTransferLogManager logManager;
    private SettingsRepository settingsRepository;

    public FileManager(SaveStrategy saveStrategy)
    {
        this.saveStrategy = saveStrategy;
        settingsRepository = new SettingsRepository();
        logManager = new FileTransferLogManager(settingsRepository.GetLogPath().GetAbsolutePath());
    }

    public void SetSaveStrategy(SaveStrategy newStrategy)
    {
        saveStrategy = newStrategy;
    }

    public void Save(string sourcePath, string destinationBasePath)
    {
        if (!Directory.Exists(destinationBasePath))
        {
            Directory.CreateDirectory(destinationBasePath);
        }

        // Générer un nom unique pour le dossier de sauvegarde
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string uniqueDestinationPath = Path.Combine(destinationBasePath, $"{timestamp}_Sauvegarde");

        // Créer le dossier de sauvegarde
        Directory.CreateDirectory(uniqueDestinationPath);

        saveStrategy.Transfer += logManager.OnTransfer;

        // Lancer la sauvegarde avec le bon dossier
        saveStrategy.Save(sourcePath, uniqueDestinationPath);
    }
}
