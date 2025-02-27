using EasySaveGUI.ViewModels.Base;

namespace EasySaveGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Represents the currently active view in the application.
        /// </summary>
        private ViewModelBase? _currentView;

        /// <summary>
        /// Gets or sets the current view model displayed in the main window.
        /// </summary>
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        /// <summary>
        /// Command to navigate to the home view.
        /// </summary>
        public RelayCommand NavigateHomeCommand { get; }

        /// <summary>
        /// Command to navigate to the settings view.
        /// </summary>
        public RelayCommand NavigateSettingsCommand { get; }

        /// <summary>
        /// Command to navigate to the create backup view.
        /// </summary>
        public RelayCommand NavigateCreateCommand { get; }

        /// <summary>
        /// Service handling navigation between views.
        /// </summary>
        private readonly NavigationService _navigationService;

        /// <summary>
        /// ViewModel for managing application settings.
        /// </summary>
        private SettingsViewModel _settingViewModel = new SettingsViewModel();

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class, setting up navigation and default views.
        /// </summary>
        public MainWindowViewModel()
        {
            _settingViewModel.SetTheme();
            _settingViewModel.SetLanguage();
            _navigationService = new NavigationService();
            _navigationService.Configure(vm => CurrentView = vm);

            NavigateHomeCommand = new RelayCommand(_ => _navigationService.Navigate(new HomeViewModel(_navigationService)));
            NavigateSettingsCommand = new RelayCommand(_ => _navigationService.Navigate(new SettingsViewModel()));
            NavigateCreateCommand = new RelayCommand(_ => _navigationService.Navigate(new CreateViewModel()));

            _navigationService.Navigate(new HomeViewModel(_navigationService)); // Default view
        }

    }
}