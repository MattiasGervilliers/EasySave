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
    internal class JobManager
    {
        private Vue vue;
        private Modele model;
        private BackupConfiguration backupConfiguration;
        private string SourcePath;
        private string CiblePath;
        private bool Quitter = false;

        public JobManager()
        {
            Vue vue = new Vue();
            Modele model = new Modele();
            //afficher choix langue

            vue.UpdateLangue();
            while (!Quitter)
            {
                vue.AfficheMenu();
                int choixAction = int.Parse(Console.ReadLine());
                switch (choixAction)
                {
                    case 1:
                        vue.AfficheNom();
                        string Name = Console.ReadLine();
                        vue.AfficheFichierSource();
                        SourcePath = Console.ReadLine();
                        Chemin CheminSource = new Chemin(SourcePath);
                        vue.AfficheFichierCible();
                        CiblePath = Console.ReadLine();
                        Chemin CheminCible = new Chemin(SourcePath);
                        vue.AfficheType();
                        BackupType backupType = DemanderBackupType();
                        backupConfiguration.Update(Name, CheminSource, CheminCible, backupType);
                        //model.AddSave();
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
        public static BackupType DemanderBackupType()
        {

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "1")
                    return BackupType.Full;
                else if (input == "2")
                    return BackupType.Incremental;
                else
                    Console.WriteLine("Entrée invalide. Veuillez entrer 1 ou 2.");
            }
        }
    }
}
