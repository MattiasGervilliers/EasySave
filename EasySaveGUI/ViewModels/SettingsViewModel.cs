using System.Windows.Input;
using BackupEngine.Settings;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;

namespace EasySaveGUI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
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
                    _settingsModel.UpdateLanguage(lang);
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
            _settingsModel = new SettingsModel();
            LoadSettings();

            SaveCommand = new CommandHandler(() => SaveSettings(), true);
        }

        private void LoadSettings()
        {
            var language = _settingsModel.GetLanguage();
            Language = language.ToString();  
            LogPath = _settingsModel.GetLogPath();
            StatePath = _settingsModel.GetStatePath();
            LogType = _settingsModel.GetLogType();
        }

        private void SaveSettings()
        {
            _settingsModel.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));
            _settingsModel.UpdateLogPath(LogPath);
            _settingsModel.UpdateStatePath(StatePath);

            // Mise à jour du type de log avec la nouvelle fonction
            _settingsModel.UpdateLogType(LogType);

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
