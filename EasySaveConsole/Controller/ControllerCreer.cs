﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole;
using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;
using EasySaveConsole.Model;
using System.Numerics;

namespace EasySaveConsole.Controller
{
    internal class ControllerCreer
    {
        private ViewCreer vue;
        private Modele model;
        private string SourcePath;
        private string CiblePath;
        private BackupConfiguration backupConfiguration;
        public ControllerCreer(Language Langue)
        {
            ViewCreer viewCreer = new ViewCreer(Langue);
            Modele model = new Modele();
            viewCreer.AfficheNom();
            string Name = Console.ReadLine();
            viewCreer.AfficheFichierSource();
            SourcePath = Console.ReadLine();
            Chemin CheminSource = new Chemin(SourcePath);
            viewCreer.AfficheFichierCible();
            CiblePath = Console.ReadLine();
            Chemin CheminCible = new Chemin(SourcePath);
            viewCreer.AfficheType();
            BackupType backupType = DemanderBackupType(Langue);
            backupConfiguration.Update(Name, CheminSource, CheminCible, backupType);
            model.AddConfig(backupConfiguration);
            Console.Clear();
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
