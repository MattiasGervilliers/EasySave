using BackupEngine.Settings;

namespace EasySaveConsole.View
{
    internal class ViewCreer
    {
        private Language Langue;
        public ViewCreer(Language langue)
        {
            this.Langue = langue;
        }
        public void AfficheNom()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Bienvenue dans le menu de création de configuration de sauvegarde");
                Console.WriteLine("Veuillez entrer le nom de la configuration : ");
            }
            else
            {
                Console.WriteLine("Welcome to the backup configuration creation menu");
                Console.WriteLine("Please enter the name of the configuration: ");
            }
        }

        public void AfficheFichierSource()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Veuillez entrer le dossier source que vous souhaitez sauvegarder : ");
            }
            else
            {
                Console.WriteLine("Please enter the source folder you want to back up: ");
            }
        }

        public void AfficheFichierCible()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Veuillez entrer le dossier cible où vous souhaitez sauvegarder : ");
            }
            else
            {
                Console.WriteLine("Please enter the target folder where you want to save: ");
            }
        }
        public void AfficheType()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Choisissez un type de sauvegarde :");
                Console.WriteLine("1 - Complète");
                Console.WriteLine("2 - Incrémentale");
                Console.Write("Votre choix (1 ou 2) : ");
            }
            else
            {
                Console.WriteLine("Choose a backup type:");
                Console.WriteLine("1 - Full");
                Console.WriteLine("2 - Incremental");
                Console.Write("Your choice (1 or 2): ");
            }
        }
        public void CreationFini()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Création d'une configuration de sauvegarde terminée !");
            }
            else
            {
                Console.WriteLine("Creation of a backup configuration completed !");
            }
        }
    }
}
