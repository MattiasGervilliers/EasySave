using BackupEngine;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class SaveController
    {
        public void AddBackupConfiguration(BackupConfiguration backupConfiguration)
        {
            BackupModel.AddConfig(backupConfiguration);

            //Console.Clear();
        }
        public void DeleteConfiguration(BackupConfiguration backupConfiguration)
        {
            BackupModel.DeleteConfig(backupConfiguration);
        }
        public BackupConfiguration? BackupExist(String Name)
        {
            if (BackupModel.FindConfig(Name ?? "") != null)
            {
                return BackupModel.FindConfig(Name ?? "");
            }
            return null;
        }
        public List<BackupConfiguration> GetConfigurations()
        {
            return BackupModel.GetConfigs();
        }
        public void LaunchBackup(string Name)
        {
            BackupConfiguration? config = BackupModel.FindConfig(Name ?? "");
            BackupModel.LaunchConfig(config);
        }

        public BackupConfiguration? GetBackupConfiguration(string name)
        {
            return BackupModel.FindConfig(name);
        }

        public void LaunchBackup(BackupConfiguration backupConfiguration)
        {
            BackupModel.LaunchConfig(backupConfiguration);
        }
    }
}
