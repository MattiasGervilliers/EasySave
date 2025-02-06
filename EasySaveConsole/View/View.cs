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
        public void AfficheChoixLangue()
        {
            Console.WriteLine("Voici les langues disponnibles / Choose a language :");
            Console.WriteLine("1 - Anglais/" + Language.English);
            Console.WriteLine("2 - Français/" + Language.French);
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
                Console.WriteLine("Veuillez entrer le fichier source que vous souhaitez sauvegarder : ");
            }
            else
            {
                Console.WriteLine("Please enter the source file you want to back up: ");
            }
        }

        public void AfficheFichierCible()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Veuillez entrer le fichier cible où vous souhaitez sauvegarder : ");
            }
            else
            {
                Console.WriteLine("Please enter the target file where you want to save: ");
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
        public void AfficheDemandeNom()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Veuillez entrer le nom de la configuration à supprimer : ");
            }
            else
            {
                Console.WriteLine("Please enter the name of the configuration to delete: ");
            }
        }

        public void AfficheConfigurations()
        {
            Console.WriteLine("LISTE DES CONFIGS");
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
