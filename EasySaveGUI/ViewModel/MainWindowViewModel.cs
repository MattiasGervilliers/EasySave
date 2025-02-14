using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySaveGUI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public RelayCommand NavigateHomeCommand { get; }
        public RelayCommand NavigateSettingsCommand { get; }

        public MainWindowViewModel()
        {
            NavigateHomeCommand = new RelayCommand(_ => CurrentView = new HomeViewModel());
            NavigateSettingsCommand = new RelayCommand(_ => CurrentView = new SettingsViewModel());
            CurrentView = new HomeViewModel(); // Default view
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
