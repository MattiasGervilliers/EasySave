using BackupEngine;
using BackupEngine.Job;
using BackupEngine.Settings;
using EasySaveGUI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasySaveGUI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly SettingsRepository _settingsRepository;
        private readonly JobManager _jobManager;
        public ObservableCollection<BackupConfiguration> BackupConfigurations { get; set; }

        // Selected configurations
        public ObservableCollection<BackupConfiguration> SelectedConfigurations { get; set; }

        // Command to launch configurations
        public ICommand LaunchConfigurationsCommand { get; }
        public ICommand LaunchConfigurationCommand { get; }

        public HomeViewModel()
        {
            _settingsRepository = new SettingsRepository();
            _jobManager = new JobManager();
            BackupConfigurations = new ObservableCollection<BackupConfiguration>(_settingsRepository.GetConfigurations());
            // Initialize SelectedConfigurations
            SelectedConfigurations = new ObservableCollection<BackupConfiguration>();

            // Initialize the command
            LaunchConfigurationsCommand = new RelayCommand(_ => LaunchConfigurations());
            LaunchConfigurationCommand = new RelayCommand(backupConfiguration => LaunchConfiguration(backupConfiguration));
        }

        // Function to launch selected configurations
        private void LaunchConfigurations()
        {
            if (SelectedConfigurations.Any())
            {
                _jobManager.LaunchBackup(SelectedConfigurations.ToList());
            }
        }

        // Function to launch a configuration
        private void LaunchConfiguration(object? backupConfiguration)
        {
            if (backupConfiguration is BackupConfiguration configuration)
            {
                _jobManager.LaunchBackup(configuration);
            }
        }
    }
}