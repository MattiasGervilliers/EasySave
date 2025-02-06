using BackupEngine;
using EasySaveConsole.View;
using BackupEngine.Settings;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class ControllerLister(Language language)
    {
        private ViewLister vue = new ViewLister(language);

        public void AfficheConfiguration()
        {
            vue.AfficheConfigurations();
            List<BackupConfiguration> configs = BackupModel.GetConfigs();
            
            foreach (BackupConfiguration config in configs)
            {
                vue.AfficheConfiguration(config);
            }

            Console.ReadLine();
            Console.Clear();
        }
    }
}
