using BackupEngine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.View
{
    internal class ViewSuppr
    {
        private Language Langue;
        public void AfficheDemandeNom(Language Langue)
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
        public void AfficheConfigIntrouvable(Language Langue)
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
