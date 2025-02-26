using BackupEngine;
using EasySaveGUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EasySaveGUI.Views
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is HomeViewModel viewModel && sender is ListBox listBox)
            {
                foreach (var item in e.RemovedItems.Cast<BackupConfiguration>())
                {
                    viewModel.SelectedConfigurations.Remove(item);
                }

                foreach (var item in e.AddedItems.Cast<BackupConfiguration>())
                {
                    if (viewModel.SelectedConfigurations.Contains(item))
                    {
                        // Deselect if already selected (toggle effect)
                        listBox.SelectedItems.Remove(item);
                    }
                    else
                    {
                        viewModel.SelectedConfigurations.Add(item);
                    }
                }
            }
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext as BackupConfiguration;  // Récupérer le DataContext

            // Afficher un message de confirmation
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette configuration ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Vérifiez que le DataContext est un ViewModel et appelez la méthode DeleteConfiguration sur celui-ci
                var viewModel = this.DataContext as HomeViewModel;  // Assurez-vous que vous obtenez le bon ViewModel
                viewModel?.DeleteConfiguration(dataContext);  // Appeler la méthode de suppression
            }
        }
    }
}
