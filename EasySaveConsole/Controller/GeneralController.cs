using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class GeneralController
    {
        private Language Langue;

        public GeneralController()
        {
            Vue vue = new Vue();
            
            // Afficher choix langue
            vue.UpdateLangue();
            while (!ChoixLangue())
            {
                Console.Clear();
                vue.AfficherErreur();
                vue.UpdateLangue();
            }

            Console.Clear();

            bool Quitter = false;

            while (!Quitter)
            {
                vue.AfficheMenu(Langue);
                string choixAction = Console.ReadLine();
                Console.Clear();

                switch (choixAction)
                {
                    case "1":
                        ControllerCreer controllerCreer = new ControllerCreer(Langue);
                        BackupModel.AddConfig(controllerCreer.GetConfiguration());
                        Console.WriteLine("creation terminée");
                        break;
                    case "2":
                        new ControllerSuppr(Langue);
                        break;
                    case "3":
                        LaunchBackupController launchBackupController = new LaunchBackupController(Langue);
                        launchBackupController.LaunchBackup();
                        break;
                    case "4":
                        ControllerLister controllerLister = new ControllerLister(Langue);
                        controllerLister.AfficheConfiguration();
                        break;
                    case "5":
                        //changer de langue
                        vue.UpdateLangue();
                        ChoixLangue();
                        Console.Clear();
                        break;
                    case "6":
                        vue.AfficheQuitter(Langue);
                        Quitter = true;
                        break;
                    default:
                        vue.AfficherErreur();
                        break;
                }
            }
        }

        bool ChoixLangue()
        {
            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    this.Langue = Language.English;
                    return true;
                case "2":
                    this.Langue = Language.French;
                    return true;
                default:
                    return false;
            }
        }
    }
}
