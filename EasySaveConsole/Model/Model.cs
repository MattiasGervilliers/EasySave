using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole;
using BackupEngine;
using LogLib;
using BackupEngine.Job;
using BackupEngine.Settings;

namespace EasySaveConsole.Model
{
    public class Modele
    {
        private JobManager jobManager;
        private SettingsRepository settingsRepository;

        public void AddConfig(BackupConfiguration backupConfiguration)
        {
            settingsRepository.AddConfiguration(backupConfiguration);
        }
        public void DeleteConfig(BackupConfiguration backupConfiguration)
        {
            settingsRepository.DeleteConfiguration(backupConfiguration);
        }
        public void LauchConfig(BackupConfiguration backupConfiguration)
        {
            jobManager.LaunchBackup(backupConfiguration);
        }
        public void ShowConfig()
        {
            settingsRepository.GetConfigurations();
        }
    }
}
