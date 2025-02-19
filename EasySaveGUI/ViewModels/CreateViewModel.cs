using System.Windows.Input;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using BackupEngine;

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

        public bool Encrypted
        {
            get => _encrypted;
            set
            {
                _encrypted = value;
                OnPropertyChanged(nameof(Encrypted));
            }
        }

        public string EncryptionKey
        {
            get => _encryptionKey;
            set
            {
                _encryptionKey = value;
                OnPropertyChanged(nameof(EncryptionKey));
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

        public ICommand CreateCommand { get; }

        public CreateViewModel()
        {
            _settingsModel = new SettingsModel();

            CreateCommand = new RelayCommand(_ => CreateConfiguration());
        }

        void CreateConfiguration()
        {
            _settingsModel.CreateConfiguration(Name, SourcePath, DestinationPath, BackupType, EncryptionKey);
        }
    }
}