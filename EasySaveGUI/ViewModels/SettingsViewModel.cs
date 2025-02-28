using System;
using System.Collections.Generic;
using System.Windows.Input;
using BackupEngine.Settings;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using LogLib;
using Microsoft.WindowsAPICodePack.Dialogs;  // Nécessaire pour le FolderPicker
using System.IO;
using MaterialDesignThemes.Wpf; // Import pour Snackbar
using System.Windows;
using System.Collections;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the settings in the application.
    /// It allows loading and saving the application's settings such as language, 
    /// log file path, state path, and log type.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private string _language;
        private string _logPath;
        private string _statePath;
        private string _logType;
        private string _theme;

        public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();

        private bool _isSnackbarActive;
        public bool IsSnackbarActive
        {
            get => _isSnackbarActive;
            set
            {
                _isSnackbarActive = value;
                OnPropertyChanged(nameof(IsSnackbarActive));
            }
        }

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
        
        public string Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        /// <summary>
        /// Property for the log file path.
        /// Allows modifying and notifying the value change.
        /// </summary>
        public string LogPath
        {
            get => _logPath;
            set
            {
                _logPath = value;
                OnPropertyChanged(nameof(LogPath));
            }
        }

        /// <summary>
        /// Property for the state file path of the configuration.
        /// Allows modifying and notifying the value change.
        /// </summary>
        public string StatePath
        {
            get => _statePath;
            set
            {
                _statePath = value;
                OnPropertyChanged(nameof(StatePath));
            }
        }

        /// <summary>
        /// Property for the log type (e.g., JSON, TXT, etc.).
        /// Allows modifying and notifying the value change.
        /// </summary>
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

        public List<string> AvailableThemes { get; } = new List<string> { "Dark", "Light" }; 

        public ICommand SaveCommand { get; }
        public ICommand BrowseLogPathCommand { get; }
        public ICommand BrowseStatePathCommand { get; }

        public SettingsViewModel()
        {
            _settingsModel = new SettingsModel();
            LoadSettings();

            SaveCommand = new RelayCommand(_ => SaveSettings());

            // Initialisation des commandes pour ouvrir l'explorateur
            BrowseLogPathCommand = new RelayCommand(_ => BrowseLogPath());
            BrowseStatePathCommand = new RelayCommand(_ => BrowseStatePath());
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

        /// <summary>
        /// Loads the current settings from the repository and assigns them to the properties.
        /// </summary>
        private void LoadSettings()
        {
            Language = _settingsModel.GetLanguage().ToString();
            Theme = _settingsModel.GetTheme().ToString();
            LogPath = _settingsModel.GetLogPath();
            StatePath = _settingsModel.GetStatePath();
            LogType = _settingsModel.GetLogType();  // Charge correctement le type de log
        }

        public void SetTheme()
        {
            string toChangeTheme = _theme;
            if (toChangeTheme == "Dark")
            {
                ResourceDictionary Theme = new ResourceDictionary() { Source = new Uri("../assets/DarkTheme.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(Theme);
            }
            else if (toChangeTheme == "Light")
            {
                ResourceDictionary Theme = new ResourceDictionary() { Source = new Uri("../assets/LightTheme.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(Theme);
            }
            else
            {
                throw new InvalidOperationException("name theme problem");
            }
        }

        public void SetLanguage()
        {
            string toChangeLanguage = _language;
            if (toChangeLanguage == "French")
            {
                ResourceDictionary languageRessource = new ResourceDictionary() { Source = new Uri("../assets/fr.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(languageRessource);
            }
            else if (toChangeLanguage == "English")
            {
                ResourceDictionary languageRessource = new ResourceDictionary() { Source = new Uri("../assets/en.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(languageRessource);
            }
            else
            {
                throw new InvalidOperationException("name language problem");
            }
        }

        /// <summary>
        /// Saves the modified settings in the repository.
        /// Updates the language, log path, state path, and log type.
        /// </summary>
        private void SaveSettings()
        {
            _settingsModel.UpdateLanguage(Language);
            _settingsModel.UpdateLogPath(LogPath);
            _settingsModel.UpdateStatePath(StatePath);
            _settingsModel.UpdateLogType(LogType);
            _settingsModel.UpdateTheme(Theme);

            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(Theme));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));

            SetTheme();
            SetLanguage();

            // Activer le Snackbar
            var message = (string)Application.Current.Resources["AlertSaveSettings"];
            MessageQueue.Enqueue(message);
            IsSnackbarActive = true;

            // Désactiver le Snackbar après un délai
            Task.Delay(3000).ContinueWith(_ => IsSnackbarActive = false);
        }
    }
}
