using System;
using System.Windows.Input;
using BackupEngine.Settings;
using BackupEngine.Shared;
using EasySaveGUI.ViewModels.Base;
using LogLib;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// ViewModel responsable de la gestion des paramètres dans l'application.
    /// Il permet de charger et de sauvegarder les paramètres de l'application comme la langue, 
    /// le chemin des fichiers de log, le chemin d'état et le type de log.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        // Repository pour accéder et mettre à jour les paramètres.
        private readonly SettingsRepository _settingsRepository;

        // Variables privées pour stocker les valeurs des paramètres.
        private string _language;
        private string _logPath;
        private string _statePath;
        private string _logType;

        /// <summary>
        /// Message d'accueil affiché sur la page des paramètres.
        /// </summary>
        public string WelcomeMessage { get; } = "Settings";

        /// <summary>
        /// Propriété qui représente la langue sélectionnée dans l'interface.
        /// Elle est liée à la langue stockée dans le repository.
        /// </summary>
        public string Language
        {
            get => _language;  // Retourne la langue actuelle
            set
            {
                _language = value;  // Modifie la langue sélectionnée
                OnPropertyChanged(nameof(Language));  // Notifie le changement de propriété

                // Convertit la chaîne de langue en énumération Language et met à jour le repository.
                if (Enum.TryParse(value, out Language lang))
                {
                    _settingsRepository.UpdateLanguage(lang);  // Met à jour la langue dans les paramètres
                }
            }
        }

        /// <summary>
        /// Propriété pour le chemin du fichier de log.
        /// Permet de modifier et de notifier le changement de valeur.
        /// </summary>
        public string LogPath
        {
            get => _logPath;  // Retourne le chemin du fichier de log
            set
            {
                _logPath = value;  // Modifie le chemin du fichier de log
                OnPropertyChanged(nameof(LogPath));  // Notifie le changement de propriété
            }
        }

        /// <summary>
        /// Propriété pour le chemin d'état du fichier de configuration.
        /// Permet de modifier et de notifier le changement de valeur.
        /// </summary>
        public string StatePath
        {
            get => _statePath;  // Retourne le chemin d'état
            set
            {
                _statePath = value;  // Modifie le chemin d'état
                OnPropertyChanged(nameof(StatePath));  // Notifie le changement de propriété
            }
        }

        /// <summary>
        /// Propriété pour le type de log (par exemple JSON, TXT, etc.).
        /// Permet de modifier et de notifier le changement de valeur.
        /// </summary>
        public string LogType
        {
            get => _logType;  // Retourne le type de log
            set
            {
                _logType = value;  // Modifie le type de log
                OnPropertyChanged(nameof(LogType));  // Notifie le changement de propriété
            }
        }

        /// <summary>
        /// Commande qui est exécutée pour sauvegarder les paramètres modifiés.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructeur qui initialise le repository des paramètres et charge les paramètres actuels.
        /// Il crée également la commande SaveCommand pour sauvegarder les paramètres.
        /// </summary>
        public SettingsViewModel()
        {
            _settingsRepository = new SettingsRepository();  // Initialise le repository des paramètres
            LoadSettings();  // Charge les paramètres actuels depuis le repository

            // Initialise la commande pour sauvegarder les paramètres.
            SaveCommand = new CommandHandler(() => SaveSettings(), true);
        }

        /// <summary>
        /// Charge les paramètres actuels depuis le repository et les affecte aux propriétés.
        /// </summary>
        private void LoadSettings()
        {
            // Charge la langue depuis les paramètres
            var language = _settingsRepository.GetLanguage();
            Language = language.ToString();

            // Charge les autres paramètres depuis le repository
            LogPath = _settingsRepository.GetLogPath().ToString();
            StatePath = _settingsRepository.GetStatePath().ToString();
            LogType = _settingsRepository.GetLogType().ToString();
        }

        /// <summary>
        /// Sauvegarde les paramètres modifiés dans le repository.
        /// Met à jour la langue, le chemin du log, le chemin d'état et le type de log.
        /// </summary>
        private void SaveSettings()
        {
            // Convertit et met à jour la langue
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));

            // Met à jour les autres paramètres
            _settingsRepository.UpdateLogPath(new CustomPath(LogPath));
            _settingsRepository.UpdateStatePath(StatePath);
            _settingsRepository.UpdateLogType((LogType)Enum.Parse(typeof(LogType), LogType));

            // Notifie le changement des propriétés après la sauvegarde
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));
        }
    }

    /// <summary>
    /// Implémentation de ICommand pour gérer l'exécution d'une commande avec une logique personnalisée.
    /// </summary>
    public class CommandHandler : ICommand
    {
        private readonly Action _execute;  // Action à exécuter lors de la commande
        private readonly bool _canExecute;  // Détermine si la commande peut être exécutée

        /// <summary>
        /// Constructeur qui initialise la commande avec l'action à exécuter et la condition d'exécution.
        /// </summary>
        public CommandHandler(Action execute, bool canExecute)
        {
            _execute = execute;  // Action à exécuter
            _canExecute = canExecute;  // Condition d'exécution
        }

        /// <summary>
        /// Retourne si la commande peut être exécutée en fonction de la condition.
        /// </summary>
        public bool CanExecute(object parameter) => _canExecute;

        /// <summary>
        /// Exécute l'action associée à la commande.
        /// </summary>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// Événement pour notifier les changements sur la condition d'exécution.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}
