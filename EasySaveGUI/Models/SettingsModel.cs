using BackupEngine;
using BackupEngine.Settings;
using BackupEngine.Shared;
using LogLib;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace EasySaveGUI.Models
{
    internal class SettingsModel
    {
        private readonly SettingsRepository _settingsRepository = new SettingsRepository();

        public List<BackupConfiguration> GetConfigurations()
        {
            return _settingsRepository.GetConfigurations();
        }

        public void UpdateLanguage(string language)
        {
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), language));
        }

        public Language GetLanguage()
        {
            return _settingsRepository.GetLanguage();
        }

        public string GetLogPath()
        {
            return _settingsRepository.GetLogPath().GetAbsolutePath();
        }

        public string GetStatePath()
        {
            return _settingsRepository.GetStatePath().GetAbsolutePath();
        }

        public string GetLogType()
        {
            return _settingsRepository.GetLogType().ToString();
        }

        public void UpdateLogPath(string logPath)
        {
            _settingsRepository.UpdateLogPath(new CustomPath(logPath));
        }

        public void UpdateStatePath(string statePath)
        {
            _settingsRepository.UpdateStatePath(statePath);
        }

        public void UpdateLogType(string logType)
        {
            _settingsRepository.UpdateLogType((LogType)Enum.Parse(typeof(LogType), logType));
        }

        public void UpdateTheme(string theme)
        {
            _settingsRepository.UpdateTheme((Theme)Enum.Parse(typeof(Theme), theme));
        }

        public Theme GetTheme()
        {
            return _settingsRepository.GetTheme();
        }

        public void CreateOrUpdateConfiguration(BackupConfiguration backupConfiguration, BackupConfiguration newBackup)
        {
            _settingsRepository.UpdateOrCreateConfiguration(backupConfiguration, newBackup);
        }

        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            _settingsRepository.DeleteConfiguration(configuration);
        }
    }
}
