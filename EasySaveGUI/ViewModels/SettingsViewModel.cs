﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using BackupEngine.Settings;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using LogLib;
using Microsoft.WindowsAPICodePack.Dialogs;  // Nécessaire pour le FolderPicker
using System.IO;

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
            var theme = _settingsRepository.GetTheme();
            Theme = theme.ToString();
            LogPath = _settingsRepository.GetLogPath().ToString();
            StatePath = _settingsRepository.GetStatePath().ToString();
            LogType = _settingsRepository.GetLogType().ToString();  // Charge correctement le type de log
        }

        private void SaveSettings()
        {
            _settingsRepository.UpdateLanguage((Language)Enum.Parse(typeof(Language), Language));
            _settingsRepository.UpdateTheme((Theme)Enum.Parse(typeof(Theme), Theme));
            _settingsRepository.UpdateLogPath(new CustomPath(LogPath));
            _settingsRepository.UpdateStatePath(StatePath);

            if (Enum.TryParse(LogType, out LogType logTypeEnum))
            {
                _settingsRepository.UpdateLogType(logTypeEnum);
            }

            // Mise à jour des propriétés après sauvegarde
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(Theme));
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
