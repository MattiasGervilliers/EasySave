using BackupEngine.Settings;

namespace EasySaveConsole.View
{
    internal class ViewSuppr (Language language)
    {
        private Language Langue = language;

        public void AfficheDemandeNom()
        {
            if (Langue == Language.French)
            {
                Console.WriteLine("Veuillez entrer le nom de la configuration à supprimer : ");
            }
            else
            {
                Console.WriteLine("Please enter the name of the configuration to delete: ");
            }
        }

        public void AfficheConfigIntrouvable()
        {
            if (Langue == Language.French)
            {
                Console.WriteLine("La configuration que vous voulez supprimer n'existe pas");
            }
            else
            {
                Console.WriteLine("The configuration you want to delete does not exist ");
            }
        }
    }
}
