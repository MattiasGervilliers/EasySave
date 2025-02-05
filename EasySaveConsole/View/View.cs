using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupEngine.Settings;

namespace EasySaveConsole.View
{
    internal class Vue
    {
        private Language Langue;
        public Vue() { 
            Language Langues = new Language();
        }
        public void ChangerLangue(int choix)
        {
            switch (choix)
            {

                case 1:
                    this.Langue = Language.English;
                break;
                case 2:
                    this.Langue = Language.French;
                break;
            }
        }
        public void AfficheMenu()
        {
            if (this.Langue == Language.French)
            {

                Console.WriteLine("Bienvenu sur EasySave ! ");
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
                Console.WriteLine("Welcom to EasySave ! ");
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
            Console.WriteLine("1 - " + Language.English);
            Console.WriteLine("2 - " + Language.French);
        }
        public void AfficheNom()
        {
            Console.WriteLine("Bienvenu dans le menu de création de configuration de sauvegarde");
            Console.WriteLine("Veullez entrer le nom de la configuration : ");
        }
        public void AfficheFichierSource()
        {
            Console.WriteLine("Veullez entrer le fichier source que vous souhaitez sauvegarder ");
        }
        public void AfficheFichierCible()
        {
            Console.WriteLine("Veullez entrer le fichier cible que vous souhaitez sauvegarder ");
        }
        public void AfficheType()
        {
            Console.WriteLine("Choisissez un type de sauvegarde :");
            Console.WriteLine("1 - Full");
            Console.WriteLine("2 - Incremental");
            Console.Write("Votre choix (1 ou 2) : ");
        }
        public void AfficheQuitter()
        {
            if (Langue == Language.French)
            {

                Console.WriteLine("Au revoir");
            }
            else
            {
                Console.WriteLine("Bye Bye");
            }
        }
    }
}
