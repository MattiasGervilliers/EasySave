using System;
using System.Collections.Generic;
using System.Windows.Input;
using BackupEngine.Settings;
using BackupEngine.Shared;
using EasySaveGUI.ViewModels.Base;
using LogLib;
using Microsoft.WindowsAPICodePack.Dialogs;  // Nécessaire pour le FolderPicker
using System.IO;

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

        // Propriétés de données
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
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

        // Liste des langues disponibles
        public List<string> AvailableLanguages { get; } = new List<string>
        {
            "English", "French"
        };

        public List<string> AvailableLogTypes { get; } = new List<string> { "Json", "Xml" };

        public ICommand SaveCommand { get; }
        public ICommand BrowseLogPathCommand { get; }
        public ICommand BrowseStatePathCommand { get; }

        public SettingsViewModel()
        {
            _settingsRepository = new SettingsRepository();
            LoadSettings();

            SaveCommand = new CommandHandler(() => SaveSettings(), true);

            // Initialisation des commandes pour ouvrir l'explorateur
            BrowseLogPathCommand = new CommandHandler(() => BrowseLogPath(), true);
            BrowseStatePathCommand = new CommandHandler(() => BrowseStatePath(), true);
        }

        private void BrowseLogPath()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true  // Spécifie que c'est un explorateur de dossiers
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                LogPath = dialog.FileName;  // Met à jour la propriété LogPath avec le chemin choisi
            }
        }

        private void BrowseStatePath()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                StatePath = dialog.FileName;  // Met à jour la propriété StatePath avec le chemin choisi
            }
        }

        private void LoadSettings()
        {
            var language = _settingsRepository.GetLanguage();
            Language = language.ToString();
            LogPath = _settingsRepository.GetLogPath().ToString();
            StatePath = _settingsRepository.GetStatePath().ToString();
            LogType = _settingsRepository.GetLogType().ToString();  // Charge correctement le type de log
        }

        private void SaveSettings()
        {
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));
            _settingsRepository.UpdateLogPath(new CustomPath(LogPath));
            _settingsRepository.UpdateStatePath(StatePath);

            if (Enum.TryParse(LogType, out LogType logTypeEnum))
            {
                _settingsRepository.UpdateLogType(logTypeEnum);
            }

            // Mise à jour des propriétés après sauvegarde
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));
        }
    }
}

// Déplacement de la classe CommandHandler en dehors de SettingsViewModel
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
