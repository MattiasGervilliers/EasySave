using EasySaveGUI.ViewModels.Base;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application's main window.
    /// It manages the views to display in the window and the navigation commands between the views.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase? _currentView;

        /// <summary>
        /// Property that exposes the current view of the application.
        /// Notifies the property change when the view changes.
        /// </summary>
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public RelayCommand NavigateHomeCommand { get; }
        public RelayCommand NavigateSettingsCommand { get; }
        public RelayCommand NavigateCreateCommand { get; }
        private readonly NavigationService _navigationService;

        private SettingsViewModel _settingViewModel = new SettingsViewModel();
        
        /// <summary>
        /// Constructor for the MainWindowViewModel class.
        /// Initializes the navigation service and navigation commands.
        /// Configures commands to navigate between views.
        /// Sets the default view to display (HomeViewModel).
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