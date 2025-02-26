using System;
using System.Windows.Input;
using BackupEngine.Settings;
using BackupEngine.Shared;
using EasySaveGUI.ViewModels.Base;
using LogLib;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the settings in the application.
    /// It allows loading and saving the application's settings such as language, 
    /// log file path, state path, and log type.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        // Repository to access and update settings.
        private readonly SettingsRepository _settingsRepository;

        // Private variables to store the settings values.
        private string _language;
        private string _logPath;
        private string _statePath;
        private string _logType;

        /// <summary>
        /// Welcome message displayed on the settings page.
        /// </summary>
        public string WelcomeMessage { get; } = "Settings";

        /// <summary>
        /// Property representing the selected language in the interface.
        /// It is bound to the language stored in the repository.
        /// </summary>
        public string Language
        {
            get => _language;  // Returns the current language
            set
            {
                _language = value;  // Modifies the selected language
                OnPropertyChanged(nameof(Language));  // Notifies the property change

                // Converts the language string to an enum Language and updates the repository.
                if (Enum.TryParse(value, out Language lang))
                {
                    _settingsRepository.UpdateLanguage(lang);  // Updates the language in the settings
                }
            }
        }

        /// <summary>
        /// Property for the log file path.
        /// Allows modifying and notifying the value change.
        /// </summary>
        public string LogPath
        {
            get => _logPath;  // Returns the log file path
            set
            {
                _logPath = value;  // Modifies the log file path
                OnPropertyChanged(nameof(LogPath));  // Notifies the property change
            }
        }

        /// <summary>
        /// Property for the state file path of the configuration.
        /// Allows modifying and notifying the value change.
        /// </summary>
        public string StatePath
        {
            get => _statePath;  // Returns the state file path
            set
            {
                _statePath = value;  // Modifies the state path
                OnPropertyChanged(nameof(StatePath));  // Notifies the property change
            }
        }

        /// <summary>
        /// Property for the log type (e.g., JSON, TXT, etc.).
        /// Allows modifying and notifying the value change.
        /// </summary>
        public string LogType
        {
            get => _logType;  // Returns the log type
            set
            {
                _logType = value;  // Modifies the log type
                OnPropertyChanged(nameof(LogType));  // Notifies the property change
            }
        }

        /// <summary>
        /// Command executed to save the modified settings.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructor that initializes the settings repository and loads the current settings.
        /// It also creates the SaveCommand to save the settings.
        /// </summary>
        public SettingsViewModel()
        {
            _settingsRepository = new SettingsRepository();  // Initializes the settings repository
            LoadSettings();  // Loads the current settings from the repository

            // Initializes the command to save the settings.
            SaveCommand = new CommandHandler(() => SaveSettings(), true);
        }

        /// <summary>
        /// Loads the current settings from the repository and assigns them to the properties.
        /// </summary>
        private void LoadSettings()
        {
            // Loads the language from the settings
            var language = _settingsRepository.GetLanguage();
            Language = language.ToString();

            // Loads other settings from the repository
            LogPath = _settingsRepository.GetLogPath().ToString();
            StatePath = _settingsRepository.GetStatePath().ToString();
            LogType = _settingsRepository.GetLogType().ToString();
        }

        /// <summary>
        /// Saves the modified settings in the repository.
        /// Updates the language, log path, state path, and log type.
        /// </summary>
        private void SaveSettings()
        {
            // Converts and updates the language
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));

            // Updates the other settings
            _settingsRepository.UpdateLogPath(new CustomPath(LogPath));
            _settingsRepository.UpdateStatePath(StatePath);
            _settingsRepository.UpdateLogType((LogType)Enum.Parse(typeof(LogType), LogType));

            // Notifies the property change after saving
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));
        }
    }

    /// <summary>
    /// Implementation of ICommand to handle the execution of a command with custom logic.
    /// </summary>
    public class CommandHandler : ICommand
    {
        private readonly Action _execute;  // Action to execute when the command is triggered
        private readonly bool _canExecute;  // Determines if the command can be executed

        /// <summary>
        /// Constructor that initializes the command with the action to execute and the execution condition.
        /// </summary>
        public CommandHandler(Action execute, bool canExecute)
        {
            _execute = execute;  // Action to execute
            _canExecute = canExecute;  // Execution condition
        }

        /// <summary>
        /// Returns whether the command can be executed based on the condition.
        /// </summary>
        public bool CanExecute(object parameter) => _canExecute;

        /// <summary>
        /// Executes the action associated with the command.
        /// </summary>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// Event to notify changes on the execution condition.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}

