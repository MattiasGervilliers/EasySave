using EasySaveGUI.ViewModels.Base;
using EasySaveGUI.Views;
using System.Windows.Controls;

namespace EasySaveGUI.ViewModels
{
    /// <summary>
    /// ViewModel principal pour la fenêtre principale de l'application.
    /// Il gère les vues à afficher dans la fenêtre et les commandes de navigation entre les vues.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        // Propriété pour stocker la vue actuellement affichée.
        private ViewModelBase? _currentView;

        /// <summary>
        /// Propriété qui expose la vue actuelle de l'application.
        /// Notifie le changement de propriété lorsque la vue change.
        /// </summary>
        public ViewModelBase? CurrentView
        {
            get => _currentView;  // Retourne la vue actuelle
            set
            {
                _currentView = value;  // Change la vue actuelle
                OnPropertyChanged(nameof(CurrentView));  // Notifie le changement de propriété
            }
        }

        // Commandes pour la navigation vers différentes vues
        public RelayCommand NavigateHomeCommand { get; }
        public RelayCommand NavigateSettingsCommand { get; }

        // Service de navigation pour gérer les transitions entre les vues
        private readonly NavigationService _navigationService;

        /// <summary>
        /// Constructeur de la classe MainWindowViewModel.
        /// Initialise le service de navigation et les commandes de navigation.
        /// Configure les commandes pour naviguer entre les vues.
        /// Définit la vue par défaut à afficher (HomeViewModel).
        /// </summary>
        public MainWindowViewModel()
        {
            _navigationService = new NavigationService();  // Initialisation du service de navigation

            // Configuration du service de navigation : lorsque la vue change, mettre à jour CurrentView
            _navigationService.Configure(vm => CurrentView = vm);

            // Commande pour naviguer vers la vue Home
            NavigateHomeCommand = new RelayCommand(_ => _navigationService.Navigate(new HomeViewModel()));

            // Commande pour naviguer vers la vue Settings
            NavigateSettingsCommand = new RelayCommand(_ => _navigationService.Navigate(new SettingsViewModel()));

            // Navigation initiale vers la vue Home (vue par défaut)
            _navigationService.Navigate(new HomeViewModel());
        }
    }
}
