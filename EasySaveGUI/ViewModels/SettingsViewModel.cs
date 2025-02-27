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
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Manages application settings and user preferences within the settings view.
        /// </summary>
        private readonly SettingsModel _settingsModel;

        /// <summary>
        /// Stores the selected application language.
        /// </summary>
        private string _language;

        /// <summary>
        /// Stores the directory path where log files are stored.
        /// </summary>
        private string _logPath;

        /// <summary>
        /// Stores the directory path where backup state files are stored.
        /// </summary>
        private string _statePath;

        /// <summary>
        /// Stores the selected log format type.
        /// </summary>
        private string _logType;

        /// <summary>
        /// Stores the selected application theme.
        /// </summary>
        private string _theme;

        /// <summary>
        /// Message queue used for displaying notifications in the UI.
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();

        /// <summary>
        /// Indicates whether the Snackbar notification is currently active.
        /// </summary>
        private bool _isSnackbarActive;

        /// <summary>
        /// Gets or sets whether the Snackbar notification is active.
        /// </summary>
        public bool IsSnackbarActive
        {
            get => _isSnackbarActive;
            set
            {
                _isSnackbarActive = value;
                OnPropertyChanged(nameof(IsSnackbarActive));
            }
        }
        /// <summary>
        /// Gets or sets the selected language for the application.
        /// </summary>
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
        /// <summary>
        /// Gets or sets the selected application theme.
        /// </summary>
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
        /// Gets or sets the directory path for log files.
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
        /// Gets or sets the directory path for state files.
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
        /// Gets or sets the selected log format type.
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
        /// <summary>
        /// List of available languages in the application.
        /// </summary>
        public List<string> AvailableLanguages { get; } = new List<string>
        {
            "English", "French"
        };
        /// <summary>
        /// List of available log format types.
        /// </summary>
        public List<string> AvailableLogTypes { get; } = new List<string> { "Json", "Xml" };

        public List<string> AvailableTheme { get; } = new List<string> { "Dark", "Light" }; 

        public ICommand SaveCommand { get; }
        public ICommand BrowseLogPathCommand { get; }
        public ICommand BrowseStatePathCommand { get; }
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            _settingsModel = new SettingsModel();
            LoadSettings();

            SaveCommand = new RelayCommand(_ => SaveSettings());

            // Initialisation des commandes pour ouvrir l'explorateur
            BrowseLogPathCommand = new RelayCommand(_ => BrowseLogPath());
            BrowseStatePathCommand = new RelayCommand(_ => BrowseStatePath());
        }
        /// <summary>
        /// Opens a dialog to browse and select a log file directory.
        /// </summary>
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
        /// <summary>
        /// Opens a dialog to browse and select a state file directory.
        /// </summary>
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
        /// Loads application settings from the settings model.
        /// </summary>
        private void LoadSettings()
        {
            Language = _settingsModel.GetLanguage().ToString();
            Theme = _settingsModel.GetTheme().ToString();
            LogPath = _settingsModel.GetLogPath();
            StatePath = _settingsModel.GetStatePath();
            LogType = _settingsModel.GetLogType();  // Charge correctement le type de log
        }
        /// <summary>
        /// Applies the selected theme to the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the theme name is invalid.</exception>
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
        /// <summary>
        /// Applies the selected language to the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the language name is invalid.</exception>
        public void SetLanguage()
        {
            string toChangeLanguage = _language;
            if (toChangeLanguage == "French")
            {
                ResourceDictionary Langue = new ResourceDictionary() { Source = new Uri("../assets/fr.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(Langue);
            }
            else if (toChangeLanguage == "English")
            {
                ResourceDictionary Langue = new ResourceDictionary() { Source = new Uri("../assets/en.xaml", UriKind.Relative) };
                App.Current.Resources.MergedDictionaries.Add(Langue);
            }
            else
            {
                throw new InvalidOperationException("name language problem");
            }
        }
        /// <summary>
        /// Saves the current application settings and updates the UI accordingly.
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
