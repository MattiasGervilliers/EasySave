using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.View;
using EasySaveConsole.Model;
using BackupEngine;
using BackupEngine.SettingsRepository;
namespace EasySaveConsole.Controller
{
    internal class ControllerSuppr
    {
        private string NomSuppr;
        private Language Langue;
        private ViewSuppr vue;
        private BackupConfiguration backupConfiguration;
        private SettingsRepository settingsRepository;
        public ControllerSuppr(Language Langue)
        {
            vue = new ViewSuppr();
            vue.AfficheDemandeNom(Langue);
            NomSuppr = Console.ReadLine();
            this.backupConfiguration = FindConfig(NomSuppr);
            //while !model.suppr(NomSuppr){ vue.AfficherConfigIntrouvable(langue)}
            // Model.suppr(NomSuppr)

        }
        public BackupConfiguration GetConfigurationSuppr()
        {
            return backupConfiguration;
        }
        public BackupConfiguration FindConfig(string Name)
        {
            List<BackupConfiguration> configurations = settingsRepository.GetConfigurations();

            foreach (BackupConfiguration config in configurations)
            {
                if (config.Name == Name)
                {
                    return config;
                }
            }
            Console.WriteLine("Erreur : Configuration non trouvée");
            return null; 
        }

    }
}
