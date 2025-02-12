using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;
using BackupEngine.Shared;

namespace EasySaveConsole.Controller
{
    internal class ControllerCreer
    {
        private ViewCreer vue;
        private readonly Language language;

        public  ControllerCreer(Language language)
        {
            this.vue = new ViewCreer(language);
            this.language = language;
        }

        public BackupConfiguration GetConfiguration()
        {
            vue.AfficheNom();
            string Name = Console.ReadLine() ?? ""; // TODO : Check if the name is valid

            vue.AfficheFichierSource();
            Chemin SourcePath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

            vue.AfficheFichierCible();
            Chemin DestinationPath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

            vue.AfficheType();
            BackupType backupType = AskBackupType();

            BackupConfiguration backupConfiguration = new BackupConfiguration
            {
                BackupType = backupType,
                DestinationPath = DestinationPath,
                SourcePath = SourcePath,
                Name = Name
            };

            //Console.Clear();
            return backupConfiguration;
        }
        private BackupType AskBackupType()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input == "1")
                    return BackupType.Full;
                else if (input == "2")
                    return BackupType.Differential;
                else
                {
                    if (language == Language.French)
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
