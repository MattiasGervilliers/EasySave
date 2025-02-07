using BackupEngine;
using BackupEngine.Settings;
using EasySaveConsole.Model;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal class LaunchBackupController (Language language)
    {
        private LaunchBackupView _view = new LaunchBackupView(language);

        public void LaunchBackup()
        {
            BackupConfiguration? config = null;

            while (config == null)
            {
                _view.AskBackupConfigurationName(); 
                string configName = Console.ReadLine();
                config = BackupModel.FindConfig(configName ?? "");

                if (config == null)
                {
                    _view.ConfigNotFound();
                }
            }
            BackupModel.LaunchConfig(config);
            _view.BackupLaunched();
        }
    }
}
