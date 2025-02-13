﻿using System;
using BackupEngine.Settings;
using BackupEngine;
using EasySaveConsole.Controller;
using EasySaveConsole.Model;
using BackupEngine.Shared;
using System.ComponentModel;

namespace EasySaveConsole.View
{
    internal class View
    {
        private Language _language;
        SaveController _saveController ;
        LanguageController _languageController = new LanguageController();
        ArgumentsController _argumentsController = new ArgumentsController();



        public View(string[] args)
        {
        
            if (args.Length > 0)
            {
                _argumentsController.UpdateArguments(args);
                _argumentsController.Execute();
                return;
            }


            Language? language = _languageController.GetLanguage();

            if (language != null)
            {
                _language = (Language)language;
            }
            else
            {
                UpdateLanguage();
                while (!ChooseLanguage())
                {
                    Console.Clear();
                    DisplayError();
                    UpdateLanguage();
                }
                _languageController.UpdateLanguage(this._language);
                _languageController.Execute(); 
            }

            Console.Clear();

            bool Quitter = false;

            while (!Quitter)
            {
                DisplayMenu();
                string choiceAction = Console.ReadLine();
                Console.Clear();

                SaveController _saveController = new SaveController(SaveAction.Add);
                switch (choiceAction)
                {
                    case "1":
                        _saveController.UpdateAction(SaveAction.Add);
                        AskBackupConfigurationCreateName();
                        string Name = Console.ReadLine() ?? ""; // TODO : Check if the name is valid
                        AskSourceFolder();
                        Chemin SourcePath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

                        AskDestinationFolder();
                        Chemin DestinationPath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

                        BackupType backupType = AskBackupType();

                        BackupConfiguration backupConfiguration = new BackupConfiguration
                        {
                            BackupType = backupType,
                            DestinationPath = DestinationPath,
                            SourcePath = SourcePath,
                            Name = Name
                        };
                        _saveController.UpdateConfiguration(backupConfiguration);
                        _saveController.Execute();
                        if (_saveController.GetResult())
                        {
                            DisplayCreateSuccess();
                        }
                        break;
                    case "2":
                        bool configurationExists = false;
                        _saveController.UpdateAction(SaveAction.Delete);
                        while (!configurationExists)
                        {
                            AskDeleteConfigurationName();
                            string configName = Console.ReadLine()?.Trim() ?? "";
                            _saveController.UpdateConfigName(configName);
                            var backupConfig = _saveController.BackupExist();
                            if (backupConfig != null)
                            {
                                _saveController.Execute();
                                configurationExists = true;
                                if (_saveController.GetResult())
                                {
                                    DisplayDeleteSuccess();
                                }
                            }
                            else
                            {
                                ConfigNotFound();
                            }
                        }
                        break;
                    case "3":
                        _saveController.UpdateAction(SaveAction.Launch);
                        configurationExists = false;                       
                        while (!configurationExists)
                        {
                            AskBackupConfigurationLaunchName();
                            string configName = Console.ReadLine()?.Trim() ?? "";
                            _saveController.UpdateConfigName(configName);
                            var backupConfig = _saveController.BackupExist();
                            if (backupConfig != null)
                            {
                                _saveController.Execute();
                                configurationExists = true;
                                DisplayLaunchSuccess();
                            }
                            else
                            {
                                ConfigNotFound();
                            }
                        }
                        break;
                    case "4":
                        List<BackupConfiguration> configs = _saveController.GetConfigurations();
                        DisplayBackupConfigurations(configs);
                        foreach (BackupConfiguration config in configs)
                        {
                            DisplayBackupConfiguration(config);
                        }
                        Console.WriteLine();
                        break;
                    case "5":
                        UpdateLanguage();
                        while (!ChooseLanguage())
                        {
                            Console.Clear();
                            DisplayError();
                            UpdateLanguage();
                        }
                        _languageController.UpdateLanguage(this._language);
                        _languageController.Execute();
                        Console.WriteLine();
                        break;
                    case "6":
                        DisplayExitMessage();
                        Quitter = true;
                        break;
                    default:
                        DisplayError();
                        break;
                }
            }
        }
        public void DisplayBackupConfiguration(BackupConfiguration configuration)
        {
            Console.WriteLine(_language == Language.French
                ? $"Nom: {configuration.Name} --- Dossier source: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Dossier de destination: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Sauvegarde {configuration.BackupType}"
                : $"Name: {configuration.Name} --- Source folder: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Destination folder: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Backup {configuration.BackupType}");            
        }
        public void DisplayDeleteSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été supprimée avec succès !\n"
                : "The configuration has been successfully deleted !\n");
        }
        public void DisplayCreateSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été créée avec succès !\n"
                : "The configuration has been successfully created !\n");
        }
        public void DisplayLaunchSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été lancée avec succès !\n"
                : "The configuration has been launched successfully !\n");
        }
        bool ChooseLanguage()
        {
            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    this._language = Language.English;
                    return true;
                case "2":
                    this._language = Language.French;
                    return true;
                default:
                    return false;
            }
        }
        public void UpdateLanguage()
        {
            Console.WriteLine("Voici les langues disponibles / Choose a language :");
            Console.WriteLine("1 - Anglais / " + Language.English);
            Console.WriteLine("2 - Français / " + Language.French);
            Console.Write("Votre choix / Your choice : ");
        }

        public void DisplayMenu()
        {
            if (_language == Language.French)
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

        public void DisplayExitMessage()
        {
            Console.WriteLine(_language == Language.French ? "Au revoir" : "Bye Bye");
        }

        public void DisplayError()
        {
            Console.WriteLine("Réponse incorrecte / Incorrect answer");
        }

        public void AskBackupConfigurationCreateName()
        {
            Console.WriteLine(_language == Language.French
                ? "Rentrez le nom de la configuration de sauvegarde à créer :"
                : "Enter the name of the backup configuration to create");
        }

        public void AskBackupConfigurationLaunchName()
        {
            Console.WriteLine(_language == Language.French
                ? "Rentrez le nom de la configuration de sauvegarde à lancer :"
                : "Enter the name of the backup configuration to launch");
        }

        public void BackupLaunched()
        {
            Console.WriteLine(_language == Language.French
                ? "La sauvegarde a été lancée"
                : "The backup has been launched");
        }

        public void ConfigNotFound()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration de sauvegarde n'a pas été trouvée"
                : "The backup configuration was not found");
        }

        public void DisplayCreateMenu()
        {
            Console.WriteLine(_language == Language.French
                ? "Bienvenue dans le menu de création de configuration de sauvegarde"
                : "Welcome to the backup configuration creation menu");
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le nom de la configuration : "
                : "Please enter the name of the configuration: ");
        }

        public void AskSourceFolder()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le dossier source que vous souhaitez sauvegarder : "
                : "Please enter the source folder you want to back up: ");
        }

        public void AskDestinationFolder()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le dossier cible où vous souhaitez sauvegarder : "
                : "Please enter the target folder where you want to save: ");
        }

        public BackupType AskBackupType()
        {
            Console.WriteLine(_language == Language.French
                ? "Choisissez un type de sauvegarde :\n1 - Complète\n2 - Incrémentale"
                : "Choose a backup type:\n1 - Full\n2 - Incremental");
            Console.Write("Votre choix (1 ou 2) / Your choice (1 or 2): ");
            while (true)
            {
                string input = Console.ReadLine();

                if (input == "1")
                    return BackupType.Full;
                else if (input == "2")
                    return BackupType.Differential;
                else
                {
                    if (_language == Language.French)
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

        public void DisplayBackupConfigurations(List<BackupConfiguration> configurations)
        {
            Console.WriteLine(_language == Language.French
                ? "Liste des configurations de sauvegarde : "
                : "List of backup configurations: ");
            if (configurations.Count == 0)
            {
                Console.WriteLine("");
            }
        }
       

        public void AskDeleteConfigurationName()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le nom de la configuration à supprimer : "
                : "Please enter the name of the configuration to delete: ");
        }

        public void DisplayConfigNotFound()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration que vous voulez supprimer n'existe pas"
                : "The configuration you want to delete does not exist");
        }
    }
}
