using BackupEngine;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EasySaveGUI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel = new SettingsModel();
        private readonly BackupModel _backupModel = new BackupModel();
        public ObservableCollection<BackupConfiguration> BackupConfigurations { get; set; }

        // Selected configurations
        public ObservableCollection<BackupConfiguration> SelectedConfigurations { get; set; }

        // Command to launch configurations
        public ICommand LaunchConfigurationsCommand { get; }
        public ICommand LaunchConfigurationCommand { get; }
        public ICommand DeleteConfigurationCommand { get; }
        public RelayCommand NavigateCreateCommand { get; }
        public RelayCommand ToggleSelectionCommand { get; }
        public ICommand EditConfigurationCommand { get; }

        private ObservableCollection<BackupConfiguration> _pausedConfigurations = [];
        private ObservableCollection<BackupConfiguration> PausedConfigurations
        {
            get => _pausedConfigurations;
            set
            {
                _pausedConfigurations = value;
                OnPropertyChanged(nameof(PausedConfigurations));
            }
        }

        private ObservableCollection<KeyValuePair<BackupConfiguration, double>> _progress;
        public ObservableCollection<KeyValuePair<BackupConfiguration, double>> Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
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

        public HomeViewModel(NavigationService navigationService)
        {
            BackupConfigurations = new ObservableCollection<BackupConfiguration>(_settingsModel.GetConfigurations());
            // Initialize SelectedConfigurations
            SelectedConfigurations = new ObservableCollection<BackupConfiguration>();

            ToggleSelectionCommand = new RelayCommand(backupConfiguration => ToggleSelection((BackupConfiguration) backupConfiguration));

            // Initialize the command
            LaunchConfigurationsCommand = new RelayCommand(_ => LaunchConfigurations());
            LaunchConfigurationCommand = new RelayCommand(backupConfiguration => LaunchConfiguration(backupConfiguration));
            NavigateCreateCommand = new RelayCommand(_ => navigationService.Navigate(new CreateViewModel()));
            EditConfigurationCommand = new RelayCommand(backupConfiguration => navigationService.Navigate(new CreateViewModel((BackupConfiguration)backupConfiguration)));

            _backupModel.ProgressUpdated += OnProgressUpdated;
            _progress = new ObservableCollection<KeyValuePair<BackupConfiguration, double>>();
            
            DeleteConfigurationCommand = new RelayCommand(obj => DeleteConfiguration((BackupConfiguration)obj));
        }

        private void PauseBackup(BackupConfiguration configuration)
        {
            _pausedConfigurations.Add(configuration);
            _backupModel.PauseBackup(configuration);

            OnPropertyChanged(nameof(PausedConfigurations));
        }

        private void OnProgressUpdated(BackupConfiguration configuration, double progress)
        {
            // Find the existing entry
            var existingEntry = _progress.FirstOrDefault(kvp => kvp.Key == configuration);

            if (!existingEntry.Equals(default(KeyValuePair<BackupConfiguration, double>)))
            {
                // Update progress in-place
                int index = _progress.IndexOf(existingEntry);
                _progress[index] = new KeyValuePair<BackupConfiguration, double>(configuration, progress);

                OnPropertyChanged(nameof(Progress));

                // If backup is completed, remove from the list to allow re-launch
                if (progress >= 100)
                {
                    _progress.Remove(existingEntry);

                    // Activer le Snackbar
                    var message = (string)Application.Current.Resources["AlertBackupFinished"];
                    // find ${0} and replace it with the configuration name
                    message = string.Format(message, configuration.Name);
                    MessageQueue.Enqueue(message);
                    IsSnackbarActive = true;

                    // Désactiver le Snackbar après un délai
                    Task.Delay(3000).ContinueWith(_ => IsSnackbarActive = false);
                }
            }
            else
            {
                // Add new entry
                _progress.Add(new KeyValuePair<BackupConfiguration, double>(configuration, progress));
            }
        }


        private void ToggleSelection(BackupConfiguration item)
        {
            if (SelectedConfigurations.Contains(item))
            {
                SelectedConfigurations.Remove(item);
            }
            else
            {
                SelectedConfigurations.Add(item);
            }
        }

        // Function to launch selected configurations
        private void LaunchConfigurations()
        {
            if (SelectedConfigurations.Any())
            {
                _backupModel.LaunchBackup(SelectedConfigurations.ToList());
            }
        }

        // Function to launch a configuration
        private void LaunchConfiguration(object? backupConfiguration)
        {
            if (backupConfiguration is BackupConfiguration configuration)
            {
                if (Progress.Any(kvp => kvp.Key == configuration))
                {
                    if (!_pausedConfigurations.Contains(configuration))
                    {
                        PauseBackup(configuration);
                        return;
                    }

                    _backupModel.ResumeBackup(configuration);
                    _pausedConfigurations.Remove(configuration);
                    return;
                }

                _backupModel.LaunchBackup(configuration);
            }
        }
        public void DeleteConfiguration(BackupConfiguration configuration)
        {
            if (configuration != null)
            {
                _settingsModel.DeleteConfiguration(configuration);  // Logic to remove from the model
                BackupConfigurations.Remove(configuration);  // Logic to remove from the list (if binding is used)
            }
        }
    }
}