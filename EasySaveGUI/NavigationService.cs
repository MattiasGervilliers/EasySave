using EasySaveGUI.ViewModels.Base;
using System.Windows.Controls;

namespace EasySaveGUI
{
    /// <summary>
    /// Service de navigation qui permet de changer la vue active dans l'application.
    /// Il permet de naviguer entre différents ViewModels et de mettre à jour l'interface utilisateur en conséquence.
    /// </summary>
    public class NavigationService
    {
        // Action qui prend un ViewModel et permet de mettre à jour la vue en conséquence.
        private Action<ViewModelBase>? _navigate;

        /// <summary>
        /// Configure le service de navigation en lui fournissant une méthode pour mettre à jour la vue.
        /// Cette méthode sera appelée chaque fois qu'une navigation est effectuée.
        /// </summary>
        /// <param name="navigate">Action qui prend un ViewModel et met à jour la vue.</param>
        public void Configure(Action<ViewModelBase> navigate) => _navigate = navigate;

        /// <summary>
        /// Navigue vers un nouveau ViewModel, ce qui déclenche la mise à jour de la vue.
        /// </summary>
        /// <param name="viewModel">Le ViewModel vers lequel naviguer.</param>
        public void Navigate(ViewModelBase viewModel) => _navigate?.Invoke(viewModel);
    }
}
