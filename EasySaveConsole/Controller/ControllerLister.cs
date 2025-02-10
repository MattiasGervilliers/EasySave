using BackupEngine;
using EasySaveConsole.View;
using BackupEngine.Settings;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class ControllerLister()
    {
        private Language _language;
        
        public void ControllerListerChangeLanguage(Language language)
        {
            _language = language;
        }
        public List<BackupConfiguration> GetConfigurations()
        {
            return BackupModel.GetConfigs();
        }
        
    }
}
