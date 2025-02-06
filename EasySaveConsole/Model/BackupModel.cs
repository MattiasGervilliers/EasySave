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
        
        public static List<BackupConfiguration> GetConfigs()
        {
            return SettingsRepository.GetConfigurations();
        }

        public static BackupConfiguration? FindConfig(string name)
        {
            return SettingsRepository.GetConfiguration(name);
        }
    }
}
