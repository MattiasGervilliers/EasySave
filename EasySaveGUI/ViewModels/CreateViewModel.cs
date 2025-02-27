using System.Windows.Input;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using BackupEngine;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace EasySaveGUI.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;

        private string _name;
        private string _sourcePath;
        private string _destinationPath;
        private bool _encrypted;
        private string _encryptionKey;
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

        public string SourcePath
        {
            get => _sourcePath;
            set
            {
                _sourcePath = value;
                OnPropertyChanged(nameof(SourcePath));
            }
        }

        public string DestinationPath
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

        public string EncryptionKey
        {
            get => _encryptionKey;
            set
            {
                _encryptionKey = value;
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

        public CreateViewModel()
        {
            _settingsModel = new SettingsModel();

            BrowseSourcePathCommand = new RelayCommand(_ => BrowseSourcePath());
            BrowseDestPathCommand = new RelayCommand(_ => BrowseDestPath());

            BackupType = BackupType.Differential;

            CreateCommand = new RelayCommand(_ => CreateConfiguration());
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
            _settingsModel.CreateConfiguration(Name, SourcePath, DestinationPath, BackupType, EncryptionKey);
        }
    }
}