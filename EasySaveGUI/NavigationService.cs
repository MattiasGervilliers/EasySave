using EasySaveGUI.ViewModels.Base;
using System.Windows.Controls;

namespace EasySaveGUI
{
    /// <summary>
    /// Navigation service that allows changing the active view in the application.
    /// It allows navigating between different ViewModels and updating the user interface accordingly.
    /// </summary>
    public class NavigationService
    {
        // Action that takes a ViewModel and updates the view accordingly.
        private Action<ViewModelBase>? _navigate;

        /// <summary>
        /// Configures the navigation service by providing a method to update the view.
        /// This method will be called whenever a navigation is performed.
        /// </summary>
        /// <param name="navigate">Action that takes a ViewModel and updates the view.</param>
        public void Configure(Action<ViewModelBase> navigate) => _navigate = navigate;

        /// <summary>
        /// Navigates to a new ViewModel, triggering the view update.
        /// </summary>
        /// <param name="viewModel">The ViewModel to navigate to.</param>
        public void Navigate(ViewModelBase viewModel) => _navigate?.Invoke(viewModel);
    }
}
