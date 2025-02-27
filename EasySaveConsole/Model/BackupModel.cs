using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;
using static System.Reflection.Metadata.BlobBuilder;

namespace EasySaveConsole.Model
{
    public static class BackupModel
    {
        private static readonly JobManager _jobManager;
        private static readonly SettingsRepository _settingsRepository;

        private static readonly Dictionary<BackupConfiguration, Job> _jobs = [];
        public static event Action<BackupConfiguration, double>? ProgressUpdated;
        static BackupModel()
        {
            _jobManager = new JobManager();
            _settingsRepository = new SettingsRepository();
        }

        public static void AddConfig(BackupConfiguration backupConfiguration)
        {
            _settingsRepository.AddConfiguration(backupConfiguration);
        }
        
        public static void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            _settingsRepository.DeleteConfiguration(backupConfiguration);
        }
        public static void LaunchConfig(BackupConfiguration backupConfiguration)
        {
            _jobManager.LaunchBackup(backupConfiguration);
        }
        /*
         */

        public static void LaunchConfigs(List<BackupConfiguration> configurations)
        {
            List<Job> jobs = _jobManager.LaunchBackup(configurations);

            for (int i = 0; i < configurations.Count; ++i)
            {
                _jobs.Add(configurations[i], jobs[i]);
                BackupConfiguration conf = configurations[i];
                //jobs[i].ProgressChanged += (sender, progress) => OnProgressChange(conf, progress);
            }
        }
        
        public static List<BackupConfiguration> GetConfigs()
        {
            return _settingsRepository.GetConfigurations();
        }

        public static BackupConfiguration? FindConfig(string name)
        {
            return _settingsRepository.GetConfiguration(name);
        }

        public static BackupConfiguration? FindConfig(int id)
        {
            return _settingsRepository.GetConfigurationById(id);
        }
        public static Language? GetLanguage()
        {
            return _settingsRepository.GetLanguage();
        }

        public static void UpdateLanguage(Language language)
        {
            _settingsRepository.UpdateLanguage(language);
        }
    }
}
