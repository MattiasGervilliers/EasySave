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
        public void AfficheMenu()
        {
            Console.WriteLine("Bienvenu sur EasySave ! ");
            Console.WriteLine("1 - Créer une configuration de sauvegarde");
            Console.WriteLine("2 - Supprimer une configuration de sauvegarde");
            Console.WriteLine("3 - Lancer une configuration de sauvegarde");
            Console.WriteLine("4 - Afficher les configurations de sauvegarde");
            Console.WriteLine("Choisissez une option :  ");
        }
        public void ShowMenu()
        {
            Console.WriteLine("Welcom to EasySave ! ");
            Console.WriteLine("1 - Create a backup configuration");
            Console.WriteLine("2 - Delete a backup configuration");
            Console.WriteLine("3 - Start a backup configuration");
            Console.WriteLine("4 - Display backup configurations");
            Console.WriteLine("Choose an option:  ");
        }
        public void AfficheChoixLangue()
        {
            Console.WriteLine("Voici les langues disponnibles : ");
            Console.WriteLine("1 - " + Langue);
            Console.WriteLine("2 - " + Langue);
        }
        public void ShowLanguageChoice() 
        {
            Console.WriteLine("Choose a language :");
            Console.WriteLine("1 - " + Langue);
            Console.WriteLine("2 - " + Langue);
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
            Console.WriteLine("Veullez entrer le type de la sauvegarde ");
        }
    }
}
