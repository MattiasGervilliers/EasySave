using System;
using System.Collections.Generic;
using System.Windows.Input;
using BackupEngine.Settings;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using LogLib;
using Microsoft.WindowsAPICodePack.Dialogs;  // Nécessaire pour le FolderPicker
using System.IO;
using System.Windows;
using System.Collections;

namespace EasySaveGUI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private string _language;
        private string _logPath;
        private string _statePath;
        private string _logType;
        private string _theme;

        public string WelcomeMessage { get; } = "Settings";

        // Propriétés de données
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
        
        public string Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
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

        public List<string> AvailableTheme { get; } = new List<string> { "Dark", "Light" }; 

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

        private void SaveSettings()
        {
            // Mise à jour des propriétés dans le modèle
            _settingsModel.UpdateLogPath(LogPath);
            _settingsModel.UpdateStatePath(StatePath);
            _settingsModel.UpdateLogType(LogType);
            _settingsModel.UpdateTheme(Theme);

            // Mise à jour des propriétés après sauvegarde
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(Theme));
            OnPropertyChanged(nameof(LogPath));
            OnPropertyChanged(nameof(StatePath));
            OnPropertyChanged(nameof(LogType));
            SetTheme();
        }
    }
}
