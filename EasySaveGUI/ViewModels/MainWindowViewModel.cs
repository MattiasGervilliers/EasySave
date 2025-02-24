using EasySaveGUI.ViewModels.Base;

namespace EasySaveGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase? _currentView;
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
        
        public MainWindowViewModel()
        {
            _settingViewModel.SetTheme();
            _navigationService = new NavigationService();
            _navigationService.Configure(vm => CurrentView = vm);

            NavigateHomeCommand = new RelayCommand(_ => _navigationService.Navigate(new HomeViewModel(_navigationService)));
            NavigateSettingsCommand = new RelayCommand(_ => _navigationService.Navigate(new SettingsViewModel()));
            NavigateCreateCommand = new RelayCommand(_ => _navigationService.Navigate(new CreateViewModel()));

            _navigationService.Navigate(new HomeViewModel(_navigationService)); // Default view
        }

    }
}