using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;

namespace EasySaveConsole.Model
{
    public static class BackupModel
    {
        private static readonly JobManager JobManager;
        private static readonly SettingsRepository SettingsRepository;

        static BackupModel()
        {
            JobManager = new JobManager();
            SettingsRepository = new SettingsRepository();
        }

        public static void AddConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.AddConfiguration(backupConfiguration);
        }
        
        public static void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            SettingsRepository.DeleteConfiguration(backupConfiguration);
        }
        
        public static void LaunchConfig(BackupConfiguration backupConfiguration)
        {
            JobManager.LaunchBackup(backupConfiguration);
        }

        public static void LaunchConfigs(List<BackupConfiguration> backupConfigurations)
        {
            JobManager.LaunchBackup(backupConfigurations);
        }
        
        public static List<BackupConfiguration> GetConfigs()
        {
            return SettingsRepository.GetConfigurations();
        }

        public static BackupConfiguration? FindConfig(string name)
        {
            return SettingsRepository.GetConfiguration(name);
        }

        public static BackupConfiguration? FindConfig(int id)
        {
            return SettingsRepository.GetConfigurationById(id);
        }

        public static Language? GetLanguage()
        {
            return SettingsRepository.GetLanguage();
        }

        public static void UpdateLanguage(Language language)
        {
            SettingsRepository.UpdateLanguage(language);
        }
    }
}
