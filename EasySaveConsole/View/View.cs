using System;
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
        SaveController saveController = new SaveController();



        public View(string[] args)
        {
        
            if (args.Length > 0)
            {
                ArgumentsController.LaunchWithArguments(args);
                return;
            }


            Language? language = GetLanguage();

            if (language != null)
            {
                _language = (Language)language;
            }
            else
            {
                // Afficher choix langue
                UpdateLanguage();
                while (!ChooseLanguage())
                {
                    Console.Clear();
                    DisplayError();
                    UpdateLanguage();
                }
            }

            Console.Clear();

            bool Quitter = false;

            while (!Quitter)
            {
                DisplayMenu();
                string choiceAction = Console.ReadLine();
                Console.Clear();

                switch (choiceAction)
                {
                    case "1":
                        AskDeleteConfigurationName();
                        string Name = Console.ReadLine() ?? ""; // TODO : Check if the name is valid

                        AskSourceFolder();
                        Chemin SourcePath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

                        AskDestinationFolder();
                        Chemin DestinationPath = new Chemin(Console.ReadLine() ?? ""); // TODO : Check if the path is valid

                        AskBackupType();
                        BackupType backupType = AskBackupType();

                        BackupConfiguration backupConfiguration = new BackupConfiguration
                        {
                            BackupType = backupType,
                            DestinationPath = DestinationPath,
                            SourcePath = SourcePath,
                            Name = Name
                        };

                        saveController.AddBackupConfiguration(backupConfiguration);
                        DisplayCreateSuccess();
                        break;
                    case "2":
                        bool configurationExists = false;

                        while (!configurationExists)
                        {
                            AskBackupConfigurationName();
                            string configName = Console.ReadLine()?.Trim() ?? ""; 

                            var backupConfig = saveController.BackupExist(configName);

                            if (backupConfig != null)
                            {
                                saveController.DeleteConfiguration(backupConfig);
                                configurationExists = true;
                                DisplayDeleteSuccess();
                            }
                            else
                            {
                                ConfigNotFound();
                            }
                        }

                        break;
                    case "3":
                        configurationExists = false;                       
                        while (!configurationExists)
                        {
                            AskBackupConfigurationName();
                            string configName = Console.ReadLine()?.Trim() ?? "";
                            var backupConfig = saveController.GetBackupConfiguration(configName);
                            if (backupConfig != null)
                            {
                                saveController.LaunchBackup(backupConfig);
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
                        List<BackupConfiguration> configs = saveController.GetConfigurations();

                        foreach (BackupConfiguration config in configs)
                        {
                            DisplayBackupConfiguration(config);
                        }
                        break;
                    case "5":
                        //changer de _language
                        UpdateLanguage();
                        ChooseLanguage();
                        Console.Clear();
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
                ? "La configuration a été supprimée avec succès"
                : "The configuration has been successfully deleted");
        }
        public void DisplayCreateSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été Créée avec succès"
                : "The configuration has been successfully created");
        }
        public void DisplayLaunchSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été lancée avec succès"
                : "The configuration has been launched successfully");
        }
        bool ChooseLanguage()
        {
            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    this._language = Language.English;
                    BackupModel.UpdateLanguage(_language);
                    return true;
                case "2":
                    this._language = Language.French;
                    BackupModel.UpdateLanguage(_language);
                    return true;
                default:
                    return false;
            }
        }
        Language? GetLanguage()
        {
            return BackupModel.GetLanguage();
        }
        public void UpdateLanguage()
        {
            Console.WriteLine("Voici les langues disponibles / Choose a language :");
            Console.WriteLine("1 - Anglais/" + Language.English);
            Console.WriteLine("2 - Français/" + Language.French);
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
                Console.Write("Choisissez une option :  ");
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
                Console.Write("Choose an option:  ");
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

        public void AskBackupConfigurationName()
        {
            Console.WriteLine(_language == Language.French
                ? "Rentrez le nom de la configuration de sauvegarde à lancer"
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
                    return BackupType.Incremental;
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

        public void CreationCompleted()
        {
            Console.WriteLine(_language == Language.French
                ? "Création d'une configuration de sauvegarde terminée !"
                : "Creation of a backup configuration completed !");
        }

        public void DisplayBackupConfigurations()
        {
            Console.WriteLine(_language == Language.French
                ? "Liste des configurations de sauvegarde : "
                : "List of backup configurations: ");
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
