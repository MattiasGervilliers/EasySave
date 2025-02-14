using System;
using BackupEngine.Settings;
using BackupEngine;
using EasySaveConsole.Controller;
using EasySaveConsole.Model;
using BackupEngine.Shared;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
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
                        bool configurationCreateExists = false;
                        _saveController.UpdateAction(SaveAction.Add);
                        AskBackupConfigurationCreateName();
                        string Name = Console.ReadLine() ?? "";
                        while (!configurationCreateExists)
                        {
                            _saveController.UpdateConfigName(Name);
                            var backupConfig = _saveController.BackupExist();
                            if (backupConfig != null || Name == "")
                            {

                                ConfigBadName();
                                AskBackupConfigurationCreateName();
                                Name = Console.ReadLine() ?? "";
                            }
                            else
                            {
                                configurationCreateExists = true;
                            }
                        }
                        AskSourceFolder();
                        CustomPath SourcePath = new CustomPath(Console.ReadLine() ?? "");
                        AskDestinationFolder();
                        CustomPath DestinationPath = new CustomPath(Console.ReadLine() ?? "");
                        BackupType backupType = AskBackupType();
                        BackupConfiguration backupConfiguration = new BackupConfiguration
                        {
                            BackupType = backupType,
                            DestinationPath = DestinationPath,
                            SourcePath = SourcePath,
                            Name = Name,
                            Encrypt = AskEncryptionPreference()
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
                        AskBackupConfigurationName();
                        string configLaunch = Console.ReadLine()?.Trim() ?? "";

                        if (Regex.IsMatch(configLaunch, @"^\d+(-\d+)*$"))
                        {
                            _argumentsController.UpdateArguments([configLaunch]);
                            _argumentsController.Execute();
                        }
                        else
                        {
                            configurationExists = false;       
                            while (!configurationExists)
                            {
                                 _saveController.UpdateConfigName(configLaunch);
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
        /// <summary>
        /// Displays an error message when an invalid name is entered.
        /// </summary>
        public void DisplayNameError()
        {
            Console.WriteLine(_language == Language.French
                ? "Le nom que vous avez saisi n'est pas valide. Veuillez en saisir un valide : "
                : "The name you entered is not valid. Please enter a valid one: ");
        }
        /// <summary>
        /// Checks if the given name is valid (not empty and matches the allowed character pattern).
        /// </summary>
        internal bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-ZÀ-ÿ\s'-]+$");
        }
        /// <summary>
        /// Displays an error message when an invalid path is entered.
        /// </summary>
        public void DisplayPathNotFound()
        {
            Console.WriteLine(_language == Language.French
                ? "Le chemin que vous avez saisi n'est pas valide. Veuillez en saisir un valide : "
                : "The path you entered is not valid. Please enter a valid one: ");
        }
        /// <summary>
        /// Displays the details of a backup configuration.
        /// </summary>
        public void DisplayBackupConfiguration(BackupConfiguration configuration)
        {
            Console.WriteLine(_language == Language.French
                ? $"Nom: {configuration.Name} --- Dossier source: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Dossier de destination: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Sauvegarde {configuration.BackupType}" + $"--- Encryption {configuration.Encrypt}"
                : $"Name: {configuration.Name} --- Source folder: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Destination folder: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Backup {configuration.BackupType}" + $"--- Encryption {configuration.Encrypt}");
        }
        /// <summary>
        /// Displays a success message when a configuration is deleted.
        /// </summary>
        public void DisplayDeleteSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été supprimée avec succès !\n"
                : "The configuration has been successfully deleted !\n");
        }
        /// <summary>
        /// Displays a success message when a configuration is created.
        /// </summary>
        public void DisplayCreateSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été créée avec succès !\n"
                : "The configuration has been successfully created !\n");
        }
        /// <summary>
        /// Displays a success message when a backup is launched.
        /// </summary>
        public void DisplayLaunchSuccess()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration a été lancée avec succès !\n"
                : "The configuration has been launched successfully !\n");
        }
        /// <summary>
        /// Handles user input to choose the application language.
        /// </summary>
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
        /// <summary>
        /// Prompts the user to select a language.
        /// </summary>
        public void UpdateLanguage()
        {
            Console.WriteLine("Voici les langues disponibles / Choose a language :");
            Console.WriteLine("1 - Anglais / " + Language.English);
            Console.WriteLine("2 - Français / " + Language.French);
            Console.Write("Votre choix / Your choice : ");
        }
        /// <summary>
        /// Displays the main menu of the application.
        /// </summary>
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
        /// <summary>
        /// Displays an exit message.
        /// </summary>
        public void DisplayExitMessage()
        {
            Console.WriteLine(_language == Language.French ? "Au revoir" : "Bye Bye");
        }
        /// <summary>
        /// Displays a generic error message for incorrect input.
        /// </summary>
        public void DisplayError()
        {
            Console.WriteLine("Réponse incorrecte / Incorrect answer");
        }

        /// <summary>
        /// Asks the user to enter the name or number of the backup configuration to launch.
        /// </summary>
        public void AskBackupConfigurationName()
        {
            Console.WriteLine(_language == Language.French
                ? "Rentrez le nom de la configuration de sauvegarde à lancer ou le(s) numéro(s) des conifgurations a lancer"
                : "Enter the name of the backup configuration to launch or the configuration number(s) to launch");
        }
      
        /// <summary>
        /// Displays a message indicating that the backup has started.
        /// </summary>
        public void BackupLaunched()
        {
            Console.WriteLine(_language == Language.French
                ? "La sauvegarde a été lancée"
                : "The backup has been launched");
        }
        /// <summary>
        /// Displays an error message when a backup configuration is not found.
        /// </summary>
        public void ConfigNotFound()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration de sauvegarde n'a pas été trouvée"
                : "The backup configuration was not found");
        }

        /// <summary>
        /// Displays the menu for creating a backup configuration.
        /// </summary>
        public void ConfigBadName()
        {
            Console.WriteLine(_language == Language.French
                ? "Le nom de configuration est vide ou existe déjà"
                : "The name of the configuration is blank or already exist");
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
        /// <summary>
        /// Asks the user to enter the source folder for backup.
        /// </summary>
        public void AskSourceFolder()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le dossier source que vous souhaitez sauvegarder : "
                : "Please enter the source folder you want to back up: ");
        }
        /// <summary>
        /// Asks the user to enter the destination folder for backup.
        /// </summary>
        public void AskDestinationFolder()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le dossier cible où vous souhaitez sauvegarder : "
                : "Please enter the target folder where you want to save: ");
        }
        /// <summary>
        /// Asks the user whether they want to encrypt the backup.
        /// </summary>
        public bool AskEncryptionPreference()
        {
            Console.WriteLine(_language == Language.French
                ? "Voulez-vous chiffrer votre sauvegarde ? (oui/non)"
                : "Do you want to encrypt your backup? (yes/no)");
            Console.Write("Votre choix / Your choice: ");

            while (true)
            {
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "oui" || input == "yes")
                    return true;
                else if (input == "non" || input == "no")
                    return false;
                else
                {
                    if (_language == Language.French)
                    {
                        Console.WriteLine("Entrée invalide. Veuillez répondre par 'oui' ou 'non'.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry. Please answer 'yes' or 'no'.");
                    }
                }
            }
        }
        /// <summary>
        /// Asks the user to choose a backup type (full or incremental).
        /// </summary>

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
        /// <summary>
        /// Displays a message indicating that the backup configuration creation is complete.
        /// </summary>
        public void CreationCompleted()
        {
            Console.WriteLine(_language == Language.French
                ? "Création d'une configuration de sauvegarde terminée !"
                : "Creation of a backup configuration completed !");
        }
        /// <summary>
        /// Displays the list of available backup configurations.
        /// </summary>
        public void DisplayBackupConfigurations(List<BackupConfiguration> configurations)
        {
            Console.WriteLine(_language == Language.French
                ? "Liste des configurations de sauvegarde : "
                : "List of backup configurations: ");
            if (configurations.Count == 0)
            {
                Console.WriteLine(_language == Language.French
                ? "Aucune configuration actuelle"
                : "No current configuration");
            }
        }
        /// <summary>
        /// Asks the user to enter the name of the configuration to delete.
        /// </summary>
        public void AskDeleteConfigurationName()
        {
            Console.WriteLine(_language == Language.French
                ? "Veuillez entrer le nom de la configuration à supprimer : "
                : "Please enter the name of the configuration to delete: ");
        }
        /// <summary>
        /// Displays an error message when the configuration to delete does not exist.
        /// </summary>
        public void DisplayConfigNotFound()
        {
            Console.WriteLine(_language == Language.French
                ? "La configuration que vous voulez supprimer n'existe pas"
                : "The configuration you want to delete does not exist");
        }
    }
}
