namespace EasySaveGUI
{
    public class NavigationService
    {
        public event Action<object>? CurrentViewChanged;
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                CurrentViewChanged?.Invoke(value);
            }
        }
    }
}
