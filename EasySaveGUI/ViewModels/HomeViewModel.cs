using BackupEngine;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using System.Collections.ObjectModel;
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
        public RelayCommand NavigateCreateCommand { get; }
        public RelayCommand ToggleSelectionCommand { get; }

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

            _backupModel.ProgressUpdated += OnProgressUpdated;

            _progress = new ObservableCollection<KeyValuePair<BackupConfiguration, double>>();
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

                // if progress >= 100, remove the entry
                if (progress >= 100)
                {
                    _progress.Remove(existingEntry);
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
                _backupModel.LaunchBackup(configuration);
            }
        }
    }
}