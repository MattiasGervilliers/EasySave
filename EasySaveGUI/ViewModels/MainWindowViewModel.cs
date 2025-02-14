using EasySaveGUI.ViewModels.Base;
using EasySaveGUI.Views;
using System.Windows.Controls;

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
        private readonly NavigationService _navigationService;

        public MainWindowViewModel()
        {
            _navigationService = new NavigationService();
            _navigationService.Configure(vm => CurrentView = vm);

            NavigateHomeCommand = new RelayCommand(_ => _navigationService.Navigate(new HomeViewModel()));
            NavigateSettingsCommand = new RelayCommand(_ => _navigationService.Navigate(new SettingsViewModel()));

            _navigationService.Navigate(new HomeViewModel()); // Default view
        }
    }
}