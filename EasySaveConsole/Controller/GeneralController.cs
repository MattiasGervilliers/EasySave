using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Model;
using EasySaveConsole.View;
using BackupEngine;
using System.Net;
using BackupEngine.Settings;

namespace EasySaveConsole.Controller
{
    internal class GeneralController
    {
        private Vue vue;
        private Modele model;
        private BackupConfiguration backupConfiguration;
        private string SourcePath;
        private string CiblePath;
        private bool Quitter = false;
        private Language Langue;

        public GeneralController()
        {
            Vue vue = new Vue();
            Modele model = new Modele();
            Language langue = new Language();
            //afficher choix langue

            vue.UpdateLangue();
            this.ChoixLangue();
            Console.Clear();
            while (!Quitter)
            {
                vue.AfficheMenu(Langue);
                int choixAction = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (choixAction)
                {
                    case 1:
                        ControllerCreer controllerCreer = new ControllerCreer(Langue);

                        break;
                    case 2:
                        ControllerSuppr controllerSuppr = new ControllerSuppr(Langue);
                        string NomSuppr = Console.ReadLine();
                        //model.RemoveSave(NomSuppr);
                        break;
                    case 3:
                        //Lancer une config
                        break;
                    case 4:
                        ControllerLister controllerLister = new ControllerLister(Langue);
                        break;
                    case 5:
                        //changer de langue
                        vue.UpdateLangue();
                        this.ChoixLangue();
                        Console.Clear();
                        break;
                    case 6:
                        vue.AfficheQuitter(Langue);
                        Quitter = true;
                        break;
                }
            }
        }
        void ChoixLangue()
        {
            int choix = int.Parse(Console.ReadLine());
            switch (choix)
            {

                case 1:
                    this.Langue = Language.English;
                    break;
                case 2:
                    this.Langue = Language.French;
                    break;
            }
        }
        
    }
}
