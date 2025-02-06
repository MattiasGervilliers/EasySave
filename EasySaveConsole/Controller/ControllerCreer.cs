using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole;
using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.SettingsRepository;
using EasySaveConsole.Model;
using System.Numerics;

namespace EasySaveConsole.Controller
{
    internal class ControllerCreer
    {
        private ViewCreer vue;
        private string SourcePath;
        private string CiblePath;
        private BackupConfiguration backupConfiguration;
        private Language langue;
        public  ControllerCreer(Language Langue)
        {
            this.vue = new ViewCreer(Langue);
            this.langue = Langue;
        }
        public BackupConfiguration GetConfiguration()
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
            BackupType backupType = DemanderBackupType(langue);
            backupConfiguration.Update(Name, CheminSource, CheminCible, backupType);
            //Console.Clear();
            return this.backupConfiguration;
        }
        public static BackupType DemanderBackupType(Language langue)
        {

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "1")
                    return BackupType.Full;
                else if (input == "2")
                    return BackupType.Incremental;
                else
                {

                    if (langue == Language.French)
                    {
                        Console.WriteLine("Entrée invalide. Veuillez entrer 1 ou 2.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry. Please enter 1 or 2.");
                    }
                }
            }
        }
    }
}
