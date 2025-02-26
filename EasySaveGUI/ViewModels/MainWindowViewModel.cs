using EasySaveGUI.ViewModels.Base;
using EasySaveGUI.Views;
using System.Windows.Controls;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application's main window.
    /// It manages the views to display in the window and the navigation commands between the views.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        // Property to store the currently displayed view.
        private ViewModelBase? _currentView;

        /// <summary>
        /// Property that exposes the current view of the application.
        /// Notifies the property change when the view changes.
        /// </summary>
        public ViewModelBase? CurrentView
        {
            get => _currentView;  // Returns the current view
            set
            {
                _currentView = value;  // Changes the current view
                OnPropertyChanged(nameof(CurrentView));  // Notifies the property change
            }
        }

        // Commands for navigating to different views
        public RelayCommand NavigateHomeCommand { get; }
        public RelayCommand NavigateSettingsCommand { get; }

        // Navigation service to handle transitions between views
        private readonly NavigationService _navigationService;

        /// <summary>
        /// Constructor for the MainWindowViewModel class.
        /// Initializes the navigation service and navigation commands.
        /// Configures commands to navigate between views.
        /// Sets the default view to display (HomeViewModel).
        /// </summary>
        public MainWindowViewModel()
        {
            _navigationService = new NavigationService();  // Initializes the navigation service

            // Configures the navigation service: when the view changes, update CurrentView
            _navigationService.Configure(vm => CurrentView = vm);

            // Command to navigate to the Home view
            NavigateHomeCommand = new RelayCommand(_ => _navigationService.Navigate(new HomeViewModel()));

            // Command to navigate to the Settings view
            NavigateSettingsCommand = new RelayCommand(_ => _navigationService.Navigate(new SettingsViewModel()));

            // Initial navigation to the Home view (default view)
            _navigationService.Navigate(new HomeViewModel());
        }
    }
}
