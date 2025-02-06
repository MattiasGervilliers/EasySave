using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole;
using EasySaveConsole.View;
using BackupEngine;

namespace EasySaveConsole.Controller
{
    internal class ControllerCreer
    {
        private ViewCreer vue;
        private string SourcePath;
        private string CiblePath;
        private BackupConfiguration backupConfiguration;
        public ControllerCreer()
        {
            ViewCreer viewCreer = new ViewCreer();
            viewCreer.AfficheNom();
            string Name = Console.ReadLine();
            viewCreer.AfficheFichierSource();
            SourcePath = Console.ReadLine();
            Chemin CheminSource = new Chemin(SourcePath);
            viewCreer.AfficheFichierCible();
            CiblePath = Console.ReadLine();
            Chemin CheminCible = new Chemin(SourcePath);
            viewCreer.AfficheType();
            BackupType backupType = DemanderBackupType();
            //backupConfiguration.Update(Name, CheminSource, CheminCible, backupType);
            //model.AddSave();
            Console.Clear();
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
