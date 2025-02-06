using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Model;
using EasySaveConsole.View;
using BackupEngine;
using System.Net;

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

        public GeneralController()
        {
            Vue vue = new Vue();
            Modele model = new Modele();
            //afficher choix langue

            vue.UpdateLangue();
            while (!Quitter)
            {
                vue.AfficheMenu();
                int choixAction = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (choixAction)
                {
                    case 1:
                        ControllerCreer controllerCreer = new ControllerCreer();
                        break;
                    case 2:
                        vue.AfficheDemandeNom();
                        string NomSuppr = Console.ReadLine();
                        //model.RemoveSave(NomSuppr);
                        break;
                    case 3:
                        vue.AfficheConfigurations();
                        break;
                    case 4:
                        //Lancer une config

                        break;
                    case 5:
                        //changer de langue
                        vue.UpdateLangue();
                        break;
                    case 6:
                        vue.AfficheQuitter();
                        Quitter = true;
                        break;
                }
            }
        }
        
    }
}
