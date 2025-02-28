using System;
using BackupEngine.Settings;
using BackupEngine;
using EasySaveConsole.Controller;
using EasySaveConsole.Model;
using BackupEngine.Shared;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using BackupEngine.Backup;
using System.Collections.Generic;
using System.ComponentModel.Design;
namespace EasySaveConsole.View
{
    internal class View
    {
        /// <summary>
        /// The current language setting used in the view.
        /// </summary>
        private Language _language;

        /// <summary>
        /// Controller responsible for managing save operations.
        /// </summary>
        SaveController _saveController;

        /// <summary>
        /// Controller responsible for handling language settings and translations.
        /// </summary>
        LanguageController _languageController = new LanguageController();

        /// <summary>
        /// Controller responsible for processing command-line arguments.
        /// </summary>
        ArgumentsController _argumentsController = new ArgumentsController();

        /// <summary>
        /// Initializes a new instance of the View class with command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
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
                            ExtensionsToSave = AskExtensions(SourcePath.ToString())
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
                                    configLaunch = Console.ReadLine()?.Trim() ?? "";

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
            string extensions = DisplayExtensions(configuration.ExtensionsToSave);
            Console.WriteLine(_language == Language.French
                ? $"Nom: {configuration.Name} --- Dossier source: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Dossier de destination: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Sauvegarde {configuration.BackupType}" + $"--- Extensions : {extensions}"
                : $"Name: {configuration.Name} --- Source folder: {configuration.SourcePath.GetAbsolutePath()} " +
                  $"--- Destination folder: {configuration.DestinationPath.GetAbsolutePath()} " +
                  $"--- Backup {configuration.BackupType}" + $"--- Extensions : {extensions}");
        }
        /// <summary>
        /// Display extension message
        /// </summary>
        /// <param name="extensions"></param>
        /// <returns></returns>
        private string? DisplayExtensions(HashSet<string> extensions)
        {
            if (extensions == null)
            {
                return null; 
            }
            return string.Join(", ", extensions);
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
        /// Asks the user to enter the name of the backup configuration to create.
        /// </summary>
        public void AskBackupConfigurationCreateName()
        {
            Console.WriteLine(_language == Language.French
                ? "Rentrez le nom de la configuration de sauvegarde à créer :"
                : "Enter the name of the backup configuration to create");
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
        /// <summary>
        /// Display te create menu
        /// </summary>
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
        /// Ask to encrypt & the extensions
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public HashSet<string>? AskExtensions(string sourcePath)
        {
            Console.WriteLine(_language == Language.French
                ? "Voulez-vous chiffrer vos données ? (oui/non) : "
                : "Do you want to encrypt your data? (yes/no) : ");

            string choice;
            do
            {
                choice = Console.ReadLine().Trim().ToLower();
            } while (choice != "oui" && choice != "non" && choice != "yes" && choice != "no");

            string encryptionKey = "";

            if (choice == "oui" || choice == "yes")
            {
                try
                {
                    ScanExtension scanner = new ScanExtension(sourcePath);
                    HashSet<string> availableExtensions = scanner.GetUniqueExtensions();

                    if (availableExtensions.Count == 0)
                    {
                        Console.WriteLine("Aucune extension trouvée dans le dossier.");
                    }

                    Console.WriteLine(_language == Language.French
                         ? "\nExtensions trouvées :"
                         : "\nFound extensions:"); foreach (string ext in availableExtensions)
                    {
                        Console.WriteLine($"- {ext}");
                    }

                    Console.WriteLine(_language == Language.French
                         ? "\nEntrez les extensions à sauvegarder :"
                         : "\nEnter the extensions to save:");
                    Console.WriteLine(_language == Language.French
                         ? "- Tapez \"tout\" ou \"all\" pour sélectionner toutes les extensions trouvées."
                         : "- Type \"all\" to select all found extensions.");
                    Console.WriteLine(_language == Language.French
                        ? "- Ou entrez manuellement les extensions séparées par des virgules (ex: .txt,.pdf,.cs)."
                        : "- Or manually enter the extensions separated by commas (e.g., .txt,.pdf,.cs).");
                    string input = Console.ReadLine().Trim();
                    HashSet<string> selectedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    if (input.Equals("tout", StringComparison.OrdinalIgnoreCase) || input.Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedExtensions = new HashSet<string>(availableExtensions, StringComparer.OrdinalIgnoreCase);
                    }
                    else if (input == "")
                    {
                        return null;
                    }
                    else
                    {
                        string[] manualExtensions = input.Split(',');

                        foreach (string ext in manualExtensions)
                        {
                            string cleanExt = ext.Trim();
                            if (!string.IsNullOrEmpty(cleanExt) && cleanExt.StartsWith("."))
                            {
                                selectedExtensions.Add(cleanExt);
                            }
                            else
                            {
                                Console.WriteLine(_language == Language.French
                                    ? $"Extension ignorée (format invalide) : {ext}"
                                    : $"Ignored extension (invalid format): {ext}");
                            }
                        }
                    }
                    return selectedExtensions;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_language == Language.French
                        ? $"Erreur : {ex.Message}"
                        : $"Error: {ex.Message}");
                    return null;
                }
            }
            else
            {
                return null;
            }

        } 


        /// <summary>
        /// Asks the user to choose a backup type (full or incremental).
        /// </summary>

        public BackupType AskBackupType()
        {
            Console.WriteLine(_language == Language.French
                ? "Choisissez un type de sauvegarde :\n1 - Complète\n2 - Différentielle"
                : "Choose a backup type:\n1 - Full\n2 - Differential");
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
