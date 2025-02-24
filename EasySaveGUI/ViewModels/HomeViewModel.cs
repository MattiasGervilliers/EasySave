using BackupEngine;
using EasySaveGUI.Models;
using EasySaveGUI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

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