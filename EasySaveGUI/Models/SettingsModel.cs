using BackupEngine;
using BackupEngine.Settings;
using BackupEngine.Shared;
using LogLib;

namespace EasySaveGUI.Models
{
    internal class SettingsModel
    {
        private readonly SettingsRepository _settingsRepository = new SettingsRepository();

        public List<BackupConfiguration> GetConfigurations()
        {
            return _settingsRepository.GetConfigurations();
        }

        public void UpdateLanguage(Language language)
        {
            _settingsRepository.UpdateLanguage(language);
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
    }
}
