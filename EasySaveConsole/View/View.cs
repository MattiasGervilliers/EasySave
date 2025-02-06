using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupEngine.Settings;

namespace EasySaveConsole.View
{
    internal class Vue
    {
        private Language Langue;
        public Vue()
        {
            Language Langues = new Language();
        }
        public void UpdateLangue()
        {
            Console.WriteLine("Voici les langues disponnibles / Choose a language :");
            Console.WriteLine("1 - Anglais/" + Language.English);
            Console.WriteLine("2 - Français/" + Language.French);
            Console.WriteLine("Votre choix / Your choice : ");

        }
        public void AfficheMenu(Language Langue)
        {
            if (Langue == Language.French)
            {

                Console.WriteLine("Bienvenue sur EasySave ! ");
                Console.WriteLine("1 - Créer une configuration de sauvegarde");
                Console.WriteLine("2 - Supprimer une configuration de sauvegarde");
                Console.WriteLine("3 - Lancer une configuration de sauvegarde");
                Console.WriteLine("4 - Afficher les configurations de sauvegarde");
                Console.WriteLine("5 - Changer la langue");
                Console.WriteLine("6 - Quitter");
                Console.WriteLine("Choisissez une option :  ");
            }
            else
            {
                Console.WriteLine("Welcome to EasySave ! ");
                Console.WriteLine("1 - Create a backup configuration");
                Console.WriteLine("2 - Delete a backup configuration");
                Console.WriteLine("3 - Start a backup configuration");
                Console.WriteLine("4 - Display backup configurations");
                Console.WriteLine("5 - Change language");
                Console.WriteLine("6 - Exit");
                Console.WriteLine("Choose an option:  ");
            }
        }
        public void AfficheQuitter(Language langue)
        {
            if (langue == Language.French)
            {

                Console.WriteLine("Au revoir");
            }
            else
            {
                Console.WriteLine("Bye Bye");
            }
        }
        public void AfficherErreur()
        {

            Console.WriteLine("Réponse incorrecte / Incorrect answer");
        }
    }
}
