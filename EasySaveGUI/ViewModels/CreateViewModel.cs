using System.Windows.Input;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using BackupEngine;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using BackupEngine.Shared;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace EasySaveGUI.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private readonly BackupConfiguration _backupConfiguration;

        private string _name;
        private string? _sourcePath;
        private string? _destinationPath;
        private BackupType _backupType;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string? SourcePath
        {
            get => _sourcePath;
            set
            {
                _sourcePath = value;
                OnPropertyChanged(nameof(SourcePath));
            }
        }

        public string? DestinationPath
        {
            get => _destinationPath;
            set
            {
                _destinationPath = value;
                OnPropertyChanged(nameof(DestinationPath));
            }
        }

        public BackupType BackupType
        {
            get => _backupType;
            set
            {
                _backupType = value;
                OnPropertyChanged(nameof(BackupType));
            }
        }

        // Liste des types de backup disponibles
        public List<string> AvailableBackupTypes { get; } = new List<string>
        {
            "Full", "Differential"
        };

        public ICommand CreateCommand { get; }
        public ICommand BrowseSourcePathCommand { get; }
        public ICommand BrowseDestPathCommand { get; }

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

        public CreateViewModel(BackupConfiguration? backupConfiguration = null)
        {
            _settingsModel = new SettingsModel();

            BrowseSourcePathCommand = new RelayCommand(_ => BrowseSourcePath());
            BrowseDestPathCommand = new RelayCommand(_ => BrowseDestPath());

            BackupType = BackupType.Differential;

            CreateCommand = new RelayCommand(_ => CreateConfiguration());
            _backupConfiguration = backupConfiguration ?? new BackupConfiguration();
            
            Name = _backupConfiguration.Name;
            if (_backupConfiguration.SourcePath != null)
                SourcePath = _backupConfiguration.SourcePath.GetAbsolutePath();
            else 
                SourcePath = "";
            if (_backupConfiguration.DestinationPath != null)
                DestinationPath = _backupConfiguration.DestinationPath.GetAbsolutePath();
            else
                DestinationPath = "";
            BackupType = _backupConfiguration.BackupType;
        }

        private void BrowseSourcePath()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SourcePath = dialog.FileName;
            }
        }

        private void BrowseDestPath()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DestinationPath = dialog.FileName;
            }
        }

        void CreateConfiguration()
        {
            BackupConfiguration newConfiguration = new BackupConfiguration();
            newConfiguration.Name = Name;
            newConfiguration.SourcePath = new CustomPath(SourcePath);
            newConfiguration.DestinationPath = new CustomPath(DestinationPath);
            newConfiguration.BackupType = BackupType;
            newConfiguration.ExtensionsToSave = new HashSet<string>();

            Debug.WriteLine("Save");
            Debug.WriteLine(_backupConfiguration.Name);
            _settingsModel.CreateOrUpdateConfiguration(_backupConfiguration, newConfiguration);

            // Activer le Snackbar
            var message = (string)Application.Current.Resources["AlertSaveConfiguration"];
            MessageQueue.Enqueue(message);
            IsSnackbarActive = true;

            // Désactiver le Snackbar après un délai
            Task.Delay(3000).ContinueWith(_ => IsSnackbarActive = false);
        }
    }
}