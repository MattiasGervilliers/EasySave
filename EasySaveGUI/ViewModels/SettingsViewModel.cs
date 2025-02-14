using System;
using System.Windows.Input;
using BackupEngine.Settings;
using BackupEngine.Shared;
using EasySaveGUI.ViewModels.Base;
using LogLib;

namespace EasySaveGUI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsRepository _settingsRepository;
        private string _language;
        private string _logPath;
        private string _statePath;
        private string _logType;

        public string WelcomeMessage { get; } = "Settings";

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));

                // Convertir la langue sélectionnée en énumération Language
                if (Enum.TryParse(value, out Language lang))
                {
                    _settingsRepository.UpdateLanguage(lang);
                }
            }
        }

        public string LogPath
        {
            get => _logPath;
            set
            {
                _logPath = value;
                OnPropertyChanged(nameof(LogPath));
            }
        }

        public string StatePath
        {
            get => _statePath;
            set
            {
                _statePath = value;
                OnPropertyChanged(nameof(StatePath));
            }
        }

        public string LogType
        {
            get => _logType;
            set
            {
                _logType = value;
                OnPropertyChanged(nameof(LogType));
            }
        }

        public ICommand SaveCommand { get; }

        public SettingsViewModel()
        {
            _settingsRepository = new SettingsRepository();
            LoadSettings();

            SaveCommand = new CommandHandler(() => SaveSettings(), true);
        }

        private void LoadSettings()
        {
            var language = _settingsRepository.GetLanguage();
            Language = language.ToString();  
            LogPath = _settingsRepository.GetLogPath().ToString();
            StatePath = _settingsRepository.GetStatePath().ToString();
            LogType = _settingsRepository.GetLogType().ToString();
        }

        private void SaveSettings()
        {
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));
            _settingsRepository.UpdateLogPath(new CustomPath(LogPath));
            _settingsRepository.UpdateStatePath(StatePath);

            // Mise à jour du type de log avec la nouvelle fonction
            _settingsRepository.UpdateLogType((LogType)Enum.Parse(typeof(LogType), LogType));

            // Notification des changements de propriétés
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));
        }

    }

    public class CommandHandler : ICommand
    {
        private readonly Action _execute;
        private readonly bool _canExecute;

        public CommandHandler(Action execute, bool canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged;
    }
}
