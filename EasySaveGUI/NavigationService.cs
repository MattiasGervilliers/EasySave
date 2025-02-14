using EasySaveGUI.ViewModels.Base;
using System.Windows.Controls;

namespace EasySaveGUI
{
    public class NavigationService
    {
        private Action<ViewModelBase>? _navigate;
        public void Configure(Action<ViewModelBase> navigate) => _navigate = navigate;
        public void Navigate(ViewModelBase viewModel) => _navigate?.Invoke(viewModel);
    }
}