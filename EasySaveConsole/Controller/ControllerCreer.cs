using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;
using BackupEngine.Shared;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class ControllerCreer
    {
        public  ControllerCreer()
        {
        }

        public void AddBackupConfiguration(BackupConfiguration backupConfiguration)
        {
            BackupModel.AddConfig(backupConfiguration);

            //Console.Clear();
        }
    }
}
