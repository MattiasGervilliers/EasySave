using System.Windows.Input;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using BackupEngine;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using BackupEngine.Shared;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Collections.ObjectModel;
using BackupEngine.Backup;

namespace EasySaveGUI.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private readonly BackupConfiguration _backupConfiguration;

        public ObservableCollection<BackupConfiguration> BackupConfigurations { get; set; }

        private string _name;
        private string? _sourcePath;
        private string? _destinationPath;
        private BackupType _backupType;
        private bool _encrypted;
        private bool _isModified;

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
                AvailableExtensions(_sourcePath);
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

        public bool Encrypted
        {
            get => _encrypted;
            set
            {
                _encrypted = value;
                OnPropertyChanged(nameof(Encrypted));
            }
        }

        public class ListItem
        {
            public bool IsSelected { get; set; }
            public string Name { get; set; }
        }
        public ObservableCollection<ListItem> ListItems { get; set; }


        // Liste des types de backup disponibles
        public List<string> AvailableBackupTypes { get; } = new List<string>
        {
            "Full", "Differential"
        };

        public ICommand CreateCommand { get; }
        public ICommand BrowseSourcePathCommand { get; }
        public ICommand BrowseDestPathCommand { get; }
        public ICommand AvailableExtensionsCommand { get; }

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

            if (backupConfiguration != null)
            {
                _isModified = true;
            }
            else
            {
                _isModified = false;
            }

            BrowseSourcePathCommand = new RelayCommand(_ => BrowseSourcePath());
            BrowseDestPathCommand = new RelayCommand(_ => BrowseDestPath());
            AvailableExtensionsCommand = new RelayCommand(_ => AvailableExtensions(SourcePath));

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

        public void AvailableExtensions(string sourcePathAvailable)
        {
            if (sourcePathAvailable != "")
            {
                ListItems = new ObservableCollection<ListItem>();

                ScanExtension scanner = new ScanExtension(sourcePathAvailable);
                HashSet<string> availableExtensions = scanner.GetUniqueExtensions();

                foreach (var extension in availableExtensions)
                {
                    ListItems.Add(new ListItem { Name = extension, IsSelected = false });
                }
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

            if (!_isModified)
            {
                BackupConfigurations = new ObservableCollection<BackupConfiguration>(_settingsModel.GetConfigurations());
                foreach (var configuration in BackupConfigurations)
                {
                    if (configuration.Name.Equals(Name) || Name == "")
                    {
                        MessageBox.Show("Invalid Name or already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            foreach (var item in ListItems)
            {
                if (item.IsSelected)
                {
                    newConfiguration.ExtensionsToSave.Add(item.Name);
                }
            }

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