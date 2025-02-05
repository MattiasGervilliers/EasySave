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
        private string SourcePath;
        private string CiblePath;

        public JobManager()
        {
            Vue vue = new Vue();
            Modele model = new Modele();

            vue.AfficheMenu();
            int choix = int.Parse(Console.ReadLine());
            if (choix == 1)
            {
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
