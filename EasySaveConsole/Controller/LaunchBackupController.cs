using BackupEngine;
using BackupEngine.Settings;
using EasySaveConsole.Model;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal class LaunchBackupController ()
    {

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
