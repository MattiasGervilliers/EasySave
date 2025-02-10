using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;
using EasySaveConsole.Model;
using System.Xml.Linq;

namespace EasySaveConsole.Controller
{
    internal class ControllerSuppr
    {

        public ControllerSuppr()
        {
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
        /*
         public ControllerSuppr(Language language)
        {
            _vue = new ViewSuppr(language);
            BackupConfiguration? configurationToDelete = null;
            while (configurationToDelete == null)
            {
                _vue.AfficheDemandeNom();
                string? name = Console.ReadLine();
                configurationToDelete = FindConfig(name);
            }
            BackupModel.DeleteConfig(configurationToDelete);
        }

        public BackupConfiguration? FindConfig(string? name)
        {
            BackupConfiguration? config = BackupModel.FindConfig(name ?? "");
            if (config != null)
            {
                return config;
            }
            _vue.AfficheConfigIntrouvable();
            return null;
        }
         */

    }
}