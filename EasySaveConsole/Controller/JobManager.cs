using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Model;
using EasySaveConsole.View;
using BackupEngine;

namespace EasySaveConsole.Controller
{
    internal class JobManager
    {
        private Vue vue;
        private Modele model;
        private BackupConfiguration backupConfiguration;


        public JobManager()
        {
            Vue vue = new Vue();
            Modele model = new Modele();
            vue.AfficheMenu();
            int choix = int.Parse(Console.ReadLine());
            if (choix == 1)
            {
                //création config
                vue.AfficheNom();
                backupConfiguration.Name = Console.ReadLine();
                vue.AfficheFichierSource();
                backupConfiguration.SetSourcePath(Console.ReadLine());
                vue.AfficheFichierCible();
                backupConfiguration.Name = Console.ReadLine();
                vue.AfficheType();
                backupConfiguration.Name = Console.ReadLine();
                //model.AddSave();
            }
            else if (choix == 2)
            {

            }
            else if (choix == 3)
            {

            }
            else if (choix == 4)
            {

            }
            else
            {
                Console.WriteLine("Veuillez séléctionner une option");
            }
        }
        public void ChoixLangue()
        {
            vue.AfficheChoixLangue();
        }
        public void CreerConfiguration()
        {
            vue.AfficheNom();

            //creer une config avec nom, src file etc ...
        }

    }
}
